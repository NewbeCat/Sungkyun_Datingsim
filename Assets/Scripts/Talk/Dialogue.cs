using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    public List<DialogueTalk> dialogueTalk;
    public DialogueType dialogueType = DialogueType.Normal;
    public ConditionBase script;  // ConditionBase를 상속한 스크립트가 드래그 드롭으로 연결이 가능해야해
    public List<Dialogue> nextDialogue;
    public List<DialogueResponse> responses;

    // 조건에 따라 다음 노드를 반환
    public Dialogue GetNextNodeBasedOnCondition()
    {
        if (dialogueType == DialogueType.ConditionBased && script != null)
        {
            return nextDialogue[script.condition_to_occur()];
        }
        return nextDialogue[0];
    }
}

//Dialogue Response
[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public Dialogue nextNode;
}

[System.Serializable]
public class DialogueTalk
{
    public string speaker;
    public string[] talkText;
}

public enum DialogueType
{
    Normal,
    ConditionBased,
    ResponseBased
}