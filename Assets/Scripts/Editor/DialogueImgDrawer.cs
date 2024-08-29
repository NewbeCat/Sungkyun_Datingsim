using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueImg))]
public class DialogueImgDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get properties
        SerializedProperty talkTextProperty = property.FindPropertyRelative("talkText");
        SerializedProperty faceProperty = property.FindPropertyRelative("face");

        // Draw talkText field
        Rect talkTextRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(talkTextRect, talkTextProperty);

        // Get the parent DialogueTalk's speaker property
        string propertyPath = property.propertyPath; // This is something like "dialogueTalk.Array.data[0].talkText.Array.data[0]"
        string speakerPath = propertyPath.Substring(0, propertyPath.LastIndexOf(".talkText")) + ".speaker";
        SerializedProperty speakerProperty = property.serializedObject.FindProperty(speakerPath);

        // Draw face field only if speaker is not "N"
        if (speakerProperty.stringValue != "N")
        {
            Rect faceRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(faceRect, faceProperty);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate the height based on whether the face field is shown
        string propertyPath = property.propertyPath;
        string speakerPath = propertyPath.Substring(0, propertyPath.LastIndexOf(".talkText")) + ".speaker";
        SerializedProperty speakerProperty = property.serializedObject.FindProperty(speakerPath);

        if (speakerProperty.stringValue != "N")
        {
            return EditorGUIUtility.singleLineHeight * 2 + 4; // talkText and face
        }
        else
        {
            return EditorGUIUtility.singleLineHeight + 2; // Only talkText
        }
    }
}
