using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    public List<DialogueTalk> dialogueTalk;

    [SerializeField] private DialogueType dialogueType0;
    public DialogueType dialogueType;
    public IsItBranch isItBranch;
    public ConditionBase script;
    public List<Dialogue> nextDialogue;
    public List<DialogueResponse> responses;

    private void OnEnable()
    {
        reset();
    }

    public void reset()
    {
        dialogueType = dialogueType0;
        isItBranch = IsItBranch.No;
        if (script != null) script.reset();
    }

    // 조건에 따라 다음 노드를 반환
    public Dialogue GetNextNodeBasedOnCondition()
    {
        if (dialogueType == DialogueType.Condition && script != null)
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
    Condition,
    Choice,
    ChoiceAll
}

public enum IsItBranch
{
    No,
    Yes
}