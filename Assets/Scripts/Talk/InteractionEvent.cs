using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private int currentDialogue = 0;
    public Dialogue[] DialogueList;

    // Trigger dialogue for this actor
    public void SpeakTo()
    {
        DialogueManager.Instance.StartDialogue(DialogueList[currentDialogue]);
    }

    public void changeNum(int num)
    {
        currentDialogue = num;
    }

    public void addNum() { currentDialogue++; }
    public void minusNum() { currentDialogue--; }
}
