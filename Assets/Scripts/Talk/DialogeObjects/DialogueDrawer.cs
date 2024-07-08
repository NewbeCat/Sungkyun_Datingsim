using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueObject.Dialogue))]
public class DialogueDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight * 6; // Base height for common fields

        var dialogueOptions = (DialogueOptions)property.FindPropertyRelative("dialogueOptions").enumValueIndex;

        switch (dialogueOptions)
        {
            case DialogueOptions.Profile:
                height += EditorGUIUtility.singleLineHeight * 2; // ProfileOptions fields
                break;
            case DialogueOptions.OnePerson:
                height += EditorGUIUtility.singleLineHeight * 4; // OnePersonOptions fields
                break;
            case DialogueOptions.TwoPerson:
                height += EditorGUIUtility.singleLineHeight * 6; // TwoPersonOptions fields
                break;
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var dialogueOptionsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var dialogueTextRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight * 2);
        var nameRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, position.width, EditorGUIUtility.singleLineHeight);
        var textStyleRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 4, position.width, EditorGUIUtility.singleLineHeight);
        var textAnimationRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 5, position.width, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(dialogueOptionsRect, property.FindPropertyRelative("dialogueOptions"), new GUIContent("Dialogue Options"));
        EditorGUI.PropertyField(dialogueTextRect, property.FindPropertyRelative("dialogueText"), new GUIContent("Dialogue Text"));
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), new GUIContent("Name"));
        EditorGUI.PropertyField(textStyleRect, property.FindPropertyRelative("textStyle"), new GUIContent("Text Style"));
        EditorGUI.PropertyField(textAnimationRect, property.FindPropertyRelative("textAnimation"), new GUIContent("Text Animation"));

        var dialogueOptions = (DialogueOptions)property.FindPropertyRelative("dialogueOptions").enumValueIndex;

        switch (dialogueOptions)
        {
            case DialogueOptions.Profile:
                var profileOptionsRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 6, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(profileOptionsRect, property.FindPropertyRelative("profileOptions").FindPropertyRelative("faceImage"), new GUIContent("Face Image"));
                break;
            case DialogueOptions.OnePerson:
                var onePersonOptionsRect1 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 6, position.width, EditorGUIUtility.singleLineHeight);
                var onePersonOptionsRect2 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 7, position.width, EditorGUIUtility.singleLineHeight);
                var onePersonOptionsRect3 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 8, position.width, EditorGUIUtility.singleLineHeight);

                EditorGUI.PropertyField(onePersonOptionsRect1, property.FindPropertyRelative("onePersonOptions").FindPropertyRelative("faceImage"), new GUIContent("Face Image"));
                EditorGUI.PropertyField(onePersonOptionsRect2, property.FindPropertyRelative("onePersonOptions").FindPropertyRelative("imagePlace"), new GUIContent("Image Place"));
                EditorGUI.PropertyField(onePersonOptionsRect3, property.FindPropertyRelative("onePersonOptions").FindPropertyRelative("imageEffect"), new GUIContent("Image Effect"));
                break;
            case DialogueOptions.TwoPerson:
                var twoPersonOptionsRect1 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 6, position.width, EditorGUIUtility.singleLineHeight);
                var twoPersonOptionsRect2 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 7, position.width, EditorGUIUtility.singleLineHeight);
                var twoPersonOptionsRect3 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 8, position.width, EditorGUIUtility.singleLineHeight);
                var twoPersonOptionsRect4 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 9, position.width, EditorGUIUtility.singleLineHeight);
                var twoPersonOptionsRect5 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 10, position.width, EditorGUIUtility.singleLineHeight);
                var twoPersonOptionsRect6 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 11, position.width, EditorGUIUtility.singleLineHeight);

                EditorGUI.PropertyField(twoPersonOptionsRect1, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("faceA"), new GUIContent("Face A"));
                EditorGUI.PropertyField(twoPersonOptionsRect2, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("effectA"), new GUIContent("Effect A"));
                EditorGUI.PropertyField(twoPersonOptionsRect3, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("faceB"), new GUIContent("Face B"));
                EditorGUI.PropertyField(twoPersonOptionsRect4, property.FindPropertyRelative("twoPersonOptions").FindPropertyRelative("effectB"), new GUIContent("Effect B"));
                break;
        }

        EditorGUI.EndProperty();
    }
}

