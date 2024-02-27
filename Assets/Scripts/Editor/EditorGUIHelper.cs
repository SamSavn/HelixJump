using UnityEngine;
using UnityEditor;

public static class EditorGUIHelper
{
#region Constants
    public const int SMALL_BTN_SIZE = 25;
    public const int MEDIUM_BTN_SIZE = 50;
    public const int BIG_BTN_SIZE = 100;

    public const int SMALL_LABEL_SIZE = 50;
    public const int MEDIUM_LABEL_SIZE = 100;
    public const int BIG_LABEL_SIZE = 200;
#endregion

#region GUI Elements
    public static void Label(string text, params GUILayoutOption[] options)
	{
        GUILayout.Label(text, options);
	}

    public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
    {
        GUILayout.Label(text, style, options);
    }

    public static void SmallLabel(string text)
    {
        Label(text, GUILayout.Width(SMALL_LABEL_SIZE));
    }

    public static void MediumLabel(string text)
    {
        Label(text, GUILayout.Width(MEDIUM_LABEL_SIZE));
    }

    public static void BigLabel(string text)
    {
        Label(text, GUILayout.Width(BIG_LABEL_SIZE));
    }

    public static void BoldLabel(string text, params GUILayoutOption[] options)
    {
        Label(text, EditorStyles.boldLabel, options);
    }

    public static bool Button(string text, params GUILayoutOption[] options)
    {
        return GUILayout.Button(text, options);
    }

    public static bool SmallButton(string text)
    {
        return Button(text, GUILayout.Width(SMALL_BTN_SIZE));
    }

    public static bool MediumButton(string text)
    {
        return Button(text, GUILayout.Width(MEDIUM_BTN_SIZE));
    }

    public static bool BigButton(string text)
    {
        return Button(text, GUILayout.Width(BIG_BTN_SIZE));
    }
#endregion
}
