using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueNameUI;
    [SerializeField] private TextMeshProUGUI dialogueTextUI;
    public void SetDialogue(DialogueObject dialogueObject, int index)
    {
        //만약 현재 페이지 안 켜졌을시 켜기
        if (index < 0 || index >= dialogueObject.dialogue.Count)
        {
            Debug.LogWarning("Index out of range.");
            return;
        }
        dialogueNameUI.text = dialogueObject.dialogue[index].name;
        dialogueTextUI.text = dialogueObject.dialogue[index].dialogueText;
    }

    public void SetNextDialogue(DialogueObject dialogueObject, ref int currentIndex)
    {
        currentIndex++;
        if (currentIndex >= dialogueObject.dialogue.Count)
        {
            currentIndex = 0; // Reset to first dialogue or handle end of dialogues
            //현재 페이지 닫기
        }

        SetDialogue(dialogueObject, currentIndex);
    }

    public void SetPreviousDialogue(DialogueObject dialogueObject, ref int currentIndex)
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = dialogueObject.dialogue.Count - 1; // Set to last dialogue or handle start of dialogues
        }

        SetDialogue(dialogueObject, currentIndex);
    }
}
