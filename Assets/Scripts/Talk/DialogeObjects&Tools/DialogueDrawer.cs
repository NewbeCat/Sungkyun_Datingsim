using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(DialogueObject.Dialogue))]
public class DialogueDrawer : PropertyDrawer
{
    private Dictionary<string, bool> showDialogueOptions = new Dictionary<string, bool>();
    private Dictionary<string, bool> showProfileOptions = new Dictionary<string, bool>();
    private Dictionary<string, bool> showOnePersonOptions = new Dictionary<string, bool>();
    private Dictionary<string, bool> showTwoPersonOptions = new Dictionary<string, bool>();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        string propertyPath = property.propertyPath;
        float height = EditorGUIUtility.singleLineHeight * 9; // Base height for text area and foldout

        if (showDialogueOptions.ContainsKey(propertyPath) && showDialogueOptions[propertyPath])
        {
            height += EditorGUIUtility.singleLineHeight * 3; // Common fields

            var dialogueOptions = (DialogueOptions)property.FindPropertyRelative("dialogueOptions").enumValueIndex;

            switch (dialogueOptions)
            {
                case DialogueOptions.Profile:
                    if (showProfileOptions.ContainsKey(propertyPath) && showProfileOptions[propertyPath])
                        height += EditorGUIUtility.singleLineHeight * 1; // ProfileOptions fields
                    break;
                case DialogueOptions.OnePerson:
                    if (showOnePersonOptions.ContainsKey(propertyPath) && showOnePersonOptions[propertyPath])
                        height += EditorGUIUtility.singleLineHeight * 4; // OnePersonOptions fields
                    break;
                case DialogueOptions.TwoPerson:
                    if (showTwoPersonOptions.ContainsKey(propertyPath) && showTwoPersonOptions[propertyPath])
                        height += EditorGUIUtility.singleLineHeight * 5; // TwoPersonOptions fields
                    break;
            }
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string propertyPath = property.propertyPath;
        if (!showDialogueOptions.ContainsKey(propertyPath))
        {
            showDialogueOptions[propertyPath] = false;
        }
        if (!showProfileOptions.ContainsKey(propertyPath))
        {
            showProfileOptions[propertyPath] = false;
        }
        if (!showOnePersonOptions.ContainsKey(propertyPath))
        {
            showOnePersonOptions[propertyPath] = false;
        }
        if (!showTwoPersonOptions.ContainsKey(propertyPath))
        {
            showTwoPersonOptions[propertyPath] = false;
        }

        EditorGUI.BeginProperty(position, label, property);

        // Find the index of this property in the parent array
        var parent = property.serializedObject.FindProperty(property.propertyPath.Substring(0, property.propertyPath.LastIndexOf('.')));
        int index = 0;
        for (int i = 0; i < parent.arraySize; i++)
        {
            if (parent.GetArrayElementAtIndex(i).propertyPath == property.propertyPath)
            {
                index = i;
                break;
            }
        }
        string dialogueLabel = $"#{index + 1}";

        // Dialogue Text Area
        var dialogueTextRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight * 6);
        EditorGUI.PropertyField(dialogueTextRect, property.FindPropertyRelative("dialogueText"), new GUIContent(dialogueLabel));

        var nameRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 6, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), new GUIContent("Name"));

        // Foldout for Common Fields
        Rect foldoutRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 7, position.width, EditorGUIUtility.singleLineHeight);
        showDialogueOptions[propertyPath] = EditorGUI.Foldout(foldoutRect, showDialogueOptions[propertyPath], "Dialogue Options", true);

        if (showDialogueOptions[propertyPath])
        {
            EditorGUI.indentLevel++;
            var dialogueOptionsRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 8, position.width, EditorGUIUtility.singleLineHeight);
            var textStyleRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 9, position.width, EditorGUIUtility.singleLineHeight);
            var textAnimationRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 10, position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(dialogueOptionsRect, property.FindPropertyRelative("dialogueOptions"), new GUIContent("Dialogue Options"));
            EditorGUI.PropertyField(textStyleRect, property.FindPropertyRelative("textStyle"), new GUIContent("Text Style"));
            EditorGUI.PropertyField(textAnimationRect, property.FindPropertyRelative("textAnimation"), new GUIContent("Text Animation"));

            var dialogueOptions = (DialogueOptions)property.FindPropertyRelative("dialogueOptions").enumValueIndex;

            switch (dialogueOptions)
            {
                case DialogueOptions.Profile:
                    showProfileOptions[propertyPath] = EditorGUI.Foldout(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 11, position.width, EditorGUIUtility.singleLineHeight), showProfileOptions[propertyPath], "Profile Options", true);
                    if (showProfileOptions[propertyPath])
                    {
                        EditorGUI.indentLevel++;
                        var profileOptionsRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 12, position.width, EditorGUIUtility.singleLineHeight);
                        EditorGUI.PropertyField(profileOptionsRect, property.FindPropertyRelative("profileOptions").FindPropertyRelative("image"), new GUIContent("Face Image"));
                        EditorGUI.indentLevel--;
                    }
                    break;
                case DialogueOptions.OnePerson:
                    showOnePersonOptions[propertyPath] = EditorGUI.Foldout(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 11, position.width, EditorGUIUtility.singleLineHeight), showOnePersonOptions[propertyPath], "One Person Options", true);
                    if (showOnePersonOptions[propertyPath])
                    {
                        EditorGUI.indentLevel++;
                        var onePersonOptionsRect1 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 12, position.width, EditorGUIUtility.singleLineHeight);
                        var onePersonOptionsRect2 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 13, position.width, EditorGUIUtility.singleLineHeight);
                        var onePersonOptionsRect3 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 14, position.width, EditorGUIUtility.singleLineHeight);
                        var onePersonOptionsRect4 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 15, position.width, EditorGUIUtility.singleLineHeight);

                        EditorGUI.PropertyField(onePersonOptionsRect1, property.FindPropertyRelative("onePersonOptions").FindPropertyRelative("focus"), new GUIContent("Focus"));
                        EditorGUI.PropertyField(onePersonOptionsRect2, property.FindPropertyRelative("onePersonOptions").FindPropertyRelative("standImage"), new GUIContent("Face Image"));
                        EditorGUI.PropertyField(onePersonOptionsRect3, property.FindPropertyRelative("onePersonOptions").FindPropertyRelative("imagePlace"), new GUIContent("Image Place"));
                        EditorGUI.PropertyField(onePersonOptionsRect4, property.FindPropertyRelative("onePersonOptions").FindPropertyRelative("imageEffect"), new GUIContent("Image Effect"));
                        EditorGUI.indentLevel--;
                    }
                    break;
                case DialogueOptions.TwoPerson:
                    showTwoPersonOptions[propertyPath] = EditorGUI.Foldout(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 11, position.width, EditorGUIUtility.singleLineHeight), showTwoPersonOptions[propertyPath], "Two Person Options", true);
                    if (showTwoPersonOptions[propertyPath])
                    {
                        EditorGUI.indentLevel++;
                        var twoPersonOptionsRect1 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 12, position.width, EditorGUIUtility.singleLineHeight);
                        var twoPersonOptionsRect2 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 13, position.width, EditorGUIUtility.singleLineHeight);
                        var twoPersonOptionsRect3 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 14, position.width, EditorGUIUtility.singleLineHeight);
                        var twoPersonOptionsRect4 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 15, position.width, EditorGUIUtility.singleLineHeight);
                        var twoPersonOptionsRect5 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 16, position.width, EditorGUIUtility.singleLineHeight);

                        EditorGUI.PropertyField(twoPersonOptionsRect1, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("focus"), new GUIContent("Focus"));
                        EditorGUI.PropertyField(twoPersonOptionsRect2, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("imageLeft"), new GUIContent("Image L"));
                        EditorGUI.PropertyField(twoPersonOptionsRect3, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("effectLeft"), new GUIContent("Effect L"));
                        EditorGUI.PropertyField(twoPersonOptionsRect4, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("imageRight"), new GUIContent("Image R"));
                        EditorGUI.PropertyField(twoPersonOptionsRect5, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("effectRight"), new GUIContent("Effect R"));
                        EditorGUI.indentLevel--;
                    }
                    break;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}