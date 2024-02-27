using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class ScriptsGeneratorEditor : EditorWindow
{
    #region Enums
    private enum ClassType
    {
        MonoBehaviour,
        ChildClass,
        BaseClass
    }

    private enum ClassAccessModifier
    {
        Public,
        Private
    }
    #endregion

    #region Constants & Fields
	private const string BASE_SCRIPTS_PATH = "Assets/Scripts";

    private const string BASE_NAMESPACE = "LKS";
	private const string MONOBEHAVIOUR = "MonoBehaviour";

	private const string SATIC_CLASS_MODIFIER = "static";
	private const string ABSTRACT_CLASS_MODIFIER = "abstract";

    private const string PUBLIC_CLASS_ACCESS_MODIFIER = "public";
    private const string PRIVATE_CLASS_ACCESS_MODIFIER = "private";

    private StringBuilder _stringBuilder = new StringBuilder();

    private ClassAccessModifier _classAccessModifier;
    private ClassType _classType;

    private string _path;
    private string _namespace;
    private string _className;
    private string _parentClassName;

    private bool _isAbstract;
    private bool _isStatic;
    #endregion

    #region Public Methods

    [MenuItem("Window/Script Generator")]
    public static void ShowWindow()
    {
        GetWindow<ScriptsGeneratorEditor>("Script Generator").Show();
    }
    #endregion

    #region GUI Methods
    private void OnGUI()
    {
        EditorGUIHelper.Label("Set the fields for creating the script");

        FolderSection();

        if (!ValidatePath())
            return;

        NamespaceSection();
        ClassSection();
        SignatureOverviewSection();

        GenerateButtonSection();
    }
    #endregion

    #region GUI Sections
    private void BeginSection(string name, bool addSpace = false)
    {
        if (addSpace)
        {
            GUILayout.Space(10);
        }

        GUILayout.BeginHorizontal();
        EditorGUIHelper.BoldLabel(name, GUILayout.Width(EditorGUIHelper.MEDIUM_LABEL_SIZE));
    }

    private void EndSection(bool endHorizontal = true)
    {
        if (endHorizontal)
        {
            GUILayout.EndHorizontal(); 
        }

        GUILayout.Space(10);
    }

    private void FolderSection()
    {
        if(EditorGUIHelper.Button("Select Folder"))
        {
            _path = EditorUtility.OpenFolderPanel("Select Folder", ValidatePath() ? _path : BASE_SCRIPTS_PATH, "");
        }

        if (ValidatePath())
        {
            EditorGUIHelper.Label($"Path: {GlobalToLocalPath(_path)}"); 
        }

        EndSection(false);
    }

    private void NamespaceSection()
    {
        BeginSection("Namespace:");
        EditorGUIHelper.Label($"{BASE_NAMESPACE}.", GUILayout.Width(30));
        _namespace = EditorGUILayout.TextField(_namespace);
        EndSection();
    }

    private void ClassSection()
    {
        _classAccessModifier = (ClassAccessModifier)EditorGUILayout.EnumPopup("Access Modifier", _classAccessModifier);
        _classType = (ClassType)EditorGUILayout.EnumPopup("Class Type", _classType);

        _isAbstract = EditorGUILayout.Toggle("Is Abstract", _isAbstract);
        _isStatic = EditorGUILayout.Toggle("Is Static", _isStatic);

        BeginSection("Class Name:", true);
        _className = EditorGUILayout.TextField(_className);

        if (_isStatic && _classType == ClassType.MonoBehaviour)
        {
            _classType = ClassType.BaseClass;
        }

        if (_classType != ClassType.BaseClass)
        {
            EditorGUIHelper.Label(":", GUILayout.Width(10));

            switch (_classType)
            {
                case ClassType.MonoBehaviour:
                default:
                    EditorGUIHelper.MediumLabel(MONOBEHAVIOUR);
                    break;

                case ClassType.ChildClass:
                    _parentClassName = EditorGUILayout.TextField(_parentClassName);
                    break;
            } 
        }

        EndSection();

    }

    private void SignatureOverviewSection()
    {
        BeginSection("Class Signature:");
        EditorGUIHelper.Label(GetClassSignature(), EditorStyles.wordWrappedLabel);
        EndSection();
    }

    private void GenerateButtonSection()
    {
        if (!ValidateName())
            return;

        if (EditorGUIHelper.Button("Generate"))
        {
            Generate();
        }
    }
    #endregion

    #region Utils Methods
    private bool ValidatePath()
    {
        return !string.IsNullOrEmpty(_path);
    }

    private bool ValidateName()
    {
        bool isNameValid = !string.IsNullOrEmpty(_className);
        bool isParentNameValid = !string.IsNullOrEmpty(_parentClassName);

        switch (_classType)
        {
            case ClassType.MonoBehaviour:
            case ClassType.BaseClass:
            default:
                return isNameValid;

            case ClassType.ChildClass:
                return isNameValid && isParentNameValid;
        }
    }

    private string GlobalToLocalPath(string path)
    {
        return path.Replace(Application.dataPath, "Assets");
    }

    private string GetNamespace()
    {
        if (string.IsNullOrEmpty(_namespace))
        {
            return BASE_NAMESPACE;
        }

        _stringBuilder.Clear();
        _stringBuilder.Append(BASE_NAMESPACE);
        _stringBuilder.Append(".");
        _stringBuilder.Append(_namespace);

        return _stringBuilder.ToString();
    }

    private string GetAccessModifier()
    {
        switch (_classAccessModifier)
        {
            case ClassAccessModifier.Public:
            default:
                return PUBLIC_CLASS_ACCESS_MODIFIER;

            case ClassAccessModifier.Private:
                return PRIVATE_CLASS_ACCESS_MODIFIER;
        }
    }

    private string GetClassModifier()
    {
        _stringBuilder.Clear();

        if(_isStatic)
        {
            AddModifier(SATIC_CLASS_MODIFIER);
        }

        if(_isAbstract)
        {
            AddModifier(ABSTRACT_CLASS_MODIFIER);
        }

        return _stringBuilder.ToString();

        void AddModifier(string modifier)
        {
            _stringBuilder.Append(' ');
            _stringBuilder.Append(modifier);
        }
    }

    private string GetClassName()
    {
        return _className;
    }

    private string GetParentClassName()
    {
        if(_classType == ClassType.BaseClass)
            return string.Empty;

        _stringBuilder.Clear();
        _stringBuilder.Append(" : ");

        switch (_classType)
        {
            case ClassType.MonoBehaviour:
            default:
                _stringBuilder.Append(MONOBEHAVIOUR);
                break;

            case ClassType.ChildClass:
                _stringBuilder.Append(_parentClassName);
                break;
        }

        return _stringBuilder.ToString();
    }

    private string GetClassSignature()
    {
        return $"{GetAccessModifier()}{GetClassModifier()} class {GetClassName()}{GetParentClassName()}";
    }
    #endregion

    #region Private Methods
    private void Generate()
    {
        string fullPath = Path.Combine(_path, $"{_className}.cs");

        if (File.Exists(fullPath))
        {
            Debug.LogWarning($"File {_className}.cs already exists at {_path}. Aborting script generation.");
            return;
        }

        try
        {
            File.WriteAllText(fullPath, GetScriptTemplate());
            AssetDatabase.Refresh();
            Debug.Log($"Script {_className}.cs generated successfully at {_path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error generating script: {e.Message}");
        }
    }
    #endregion

    #region Templates
    private string GetScriptTemplate()
    {
        return $@"using UnityEngine;

namespace {GetNamespace()}
{{
    {GetClassSignature()}
    {{
        
    }}
}}";
    }
    #endregion
}
