using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public void SetDialogue(TextMeshProUGUI dialogueTextUI, DialogueObject dialogueObject, int index)
    {
        if (index < 0 || index >= dialogueObject.dialogue.Count)
        {
            Debug.LogWarning("Index out of range.");
            return;
        }

        dialogueTextUI.text = dialogueObject.dialogue[index].dialogueText;
    }

    public void SetNextDialogue(TextMeshProUGUI dialogueTextUI, DialogueObject dialogueObject, ref int currentIndex)
    {
        currentIndex++;
        if (currentIndex >= dialogueObject.dialogue.Count)
        {
            currentIndex = 0; // Reset to first dialogue or handle end of dialogues
        }

        SetDialogue(dialogueTextUI, dialogueObject, currentIndex);
    }

    public void SetPreviousDialogue(TextMeshProUGUI dialogueTextUI, DialogueObject dialogueObject, ref int currentIndex)
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = dialogueObject.dialogue.Count - 1; // Set to last dialogue or handle start of dialogues
        }

        SetDialogue(dialogueTextUI, dialogueObject, currentIndex);
    }
}
