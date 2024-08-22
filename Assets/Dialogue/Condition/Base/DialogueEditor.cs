using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    SerializedProperty talk;
    SerializedProperty type;
    SerializedProperty script;
    SerializedProperty nextDialogue;
    SerializedProperty responses;

    void OnEnable()
    {
        talk = serializedObject.FindProperty("dialogueTalk");
        type = serializedObject.FindProperty("dialogueType");
        script = serializedObject.FindProperty("script");
        nextDialogue = serializedObject.FindProperty("nextDialogue");
        responses = serializedObject.FindProperty("responses");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(talk);
        EditorGUILayout.PropertyField(type);

        // Correctly compare the enum value using enumValueIndex
        if (type.enumValueIndex == (int)DialogueType.ConditionBased)
        {
            EditorGUILayout.PropertyField(script);
            EditorGUILayout.PropertyField(nextDialogue, true);
        }
        else if (type.enumValueIndex == (int)DialogueType.ResponseBased)
        {
            EditorGUILayout.PropertyField(script);
            EditorGUILayout.PropertyField(responses, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
