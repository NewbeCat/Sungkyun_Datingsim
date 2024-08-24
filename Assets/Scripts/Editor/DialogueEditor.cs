using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    SerializedProperty dialogueTalk;
    SerializedProperty dialogueType0;
    SerializedProperty script;
    SerializedProperty nextDialogue;
    SerializedProperty responses;

    void OnEnable()
    {
        dialogueTalk = serializedObject.FindProperty("dialogueTalk");
        dialogueType0 = serializedObject.FindProperty("dialogueType0");
        script = serializedObject.FindProperty("script");
        nextDialogue = serializedObject.FindProperty("nextDialogue");
        responses = serializedObject.FindProperty("responses");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(dialogueTalk);
        EditorGUILayout.PropertyField(dialogueType0);

        // Correctly compare the enum value using enumValueIndex
        if (dialogueType0.enumValueIndex == (int)DialogueType.Condition)
        {
            EditorGUILayout.PropertyField(script);
            EditorGUILayout.PropertyField(nextDialogue, true);
        }
        else if (dialogueType0.enumValueIndex == (int)DialogueType.Choice)
        {
            EditorGUILayout.PropertyField(script);
            EditorGUILayout.PropertyField(responses, true);
        }
        else if (dialogueType0.enumValueIndex == (int)DialogueType.ChoiceAll)
        {
            EditorGUILayout.PropertyField(script);
            EditorGUILayout.PropertyField(responses, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}