using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ScriptsGeneratorEditor : EditorWindow
{
#region Enums
    private enum ClassType
    {
        MonoBehaviour,
        ChildClass,
        BaseClass,
        ScriptableObject,
        Interface
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
	private const string MONO_BEHAVIOUR = "MonoBehaviour";
	private const string SCRIPTABLE_OBJ = "ScriptableObject";

	private const string SATIC_CLASS_MODIFIER = "static";
	private const string ABSTRACT_CLASS_MODIFIER = "abstract";

    private const string PUBLIC_CLASS_ACCESS_MODIFIER = "public";
    private const string PRIVATE_CLASS_ACCESS_MODIFIER = "private";

    private const string CLASS = "class";
    private const string INTERFACE = "interface";

    private StringBuilder _stringBuilder = new StringBuilder();

    private ClassAccessModifier _classAccessModifier;
    private ClassType _classType;

    private string _path;
    private string _namespace;
    private string _className;
    private string _parentClassName;

    private string _scriptableFileName;
    private string _scriptableMenuName;

    private bool _isAbstract;
    private bool _isStatic;
#endregion

#region Properties
    private bool CanBeAbstract => _classType != ClassType.ScriptableObject &&
                                  _classType != ClassType.Interface;
    private bool CanBeStatic => _classType == ClassType.BaseClass;
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

        if (CanBeAbstract)
        {
            _isAbstract = EditorGUILayout.Toggle("Is Abstract", _isAbstract); 
        }

        if (CanBeStatic)
        {
            _isStatic = EditorGUILayout.Toggle("Is Static", _isStatic); 
        }

        BeginSection("Class Name:", true);
        _className = EditorGUILayout.TextField(_className);

        if (_classType != ClassType.BaseClass)
        {
            EditorGUIHelper.Label(":", GUILayout.Width(10));

            switch (_classType)
            {
                case ClassType.MonoBehaviour:
                default:
                    EditorGUIHelper.MediumLabel(MONO_BEHAVIOUR);
                    break;

                case ClassType.ScriptableObject:
                    EditorGUIHelper.MediumLabel(SCRIPTABLE_OBJ);
                    break;

                case ClassType.ChildClass:
                case ClassType.Interface:
                    _parentClassName = EditorGUILayout.TextField(_parentClassName);
                    break;
            } 
        }

        EndSection();

        if(_classType == ClassType.ScriptableObject)
        {
            _scriptableFileName = EditorGUILayout.TextField("File Name", _scriptableFileName);
            _scriptableMenuName = EditorGUILayout.TextField("Menu Name", _scriptableMenuName);
        }
    }

    private void SignatureOverviewSection()
    {
        BeginSection("Class Signature:", _classType == ClassType.ScriptableObject);
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

    private string GetClassType()
    {
        if(_classType == ClassType.Interface)
        {
            return INTERFACE;
        }

        return CLASS;
    }

    private string GetClassName()
    {
        return _className;
    }

    private string GetParentClassName()
    {
        if(_classType == ClassType.BaseClass)
            return string.Empty;

        string parentClassName;
        _stringBuilder.Clear();

        switch (_classType)
        {
            case ClassType.MonoBehaviour:
            default:
                parentClassName = MONO_BEHAVIOUR;
                break;

            case ClassType.ScriptableObject:
                parentClassName = SCRIPTABLE_OBJ;
                break;

            case ClassType.ChildClass:
            case ClassType.Interface:
                parentClassName = _parentClassName;
                break;
        }

        if (!string.IsNullOrEmpty(parentClassName))
        {
            _stringBuilder.Append(" : ");
            _stringBuilder.Append(parentClassName);
        }
        else
        {
            return parentClassName;
        }

        return _stringBuilder.ToString();
    }

    private string GetCreateMenu()
    {
        if (_classType != ClassType.ScriptableObject)
            return string.Empty;

        return $"\n[CreateAssetMenu(fileName = \"{_scriptableFileName}\", menuName = \"{_scriptableMenuName}\")]";
    }

    private string GetClassSignature()
    {
        return $"{GetAccessModifier()}{GetClassModifier()} {GetClassType()} {GetClassName()}{GetParentClassName()}";
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
{{{GetCreateMenu()}
    {GetClassSignature()}
    {{
        
    }}
}}";
    }
#endregion
}
