using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] GameObject scriptholder;
    [SerializeField] GameObject dialoguePanel;
    DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = dialoguePanel.GetComponent<DialogueManager>();
    }

    public void Prompt() // start는 어디서부터 다시 시작하는지 ID 숫자입니다.
    {
        dialogueManager.ShowDialogue(scriptholder.GetComponent<InteractionEvent>().GetDialogue());
    }

}
