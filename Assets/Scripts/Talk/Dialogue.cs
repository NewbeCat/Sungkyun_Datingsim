using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    public List<DialogueTalk> dialogueTalk;

    [SerializeField] private DialogueType dialogueType0;
    [SerializeField] private IsItBranch isItBranch0;
    public DialogueType dialogueType;
    public IsItBranch isItBranch;
    public ConditionBase script;  // ConditionBase를 상속한 스크립트가 드래그 드롭으로 연결이 가능해야해
    public List<Dialogue> nextDialogue;
    public List<DialogueResponse> responses;

    private void OnEnable()
    {
        reset();
    }

    public void reset()
    {
        dialogueType = dialogueType0;
        isItBranch = isItBranch0;
        if (script != null) script.reset();
    }

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
    public Dialogue nextNode = null;
}

[System.Serializable]
public class DialogueTalk
{
    public string speaker = null;
    public string[] talkText;
}

public enum DialogueType
{
    Normal,
    ConditionBased,
    ResponseBased
}

public enum IsItBranch
{
    No,
    Yes
}