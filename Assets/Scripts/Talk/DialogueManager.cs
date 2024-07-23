using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject DialogueBar;
    OpenCloseWindow windowOnOff;

    [SerializeField] TextMeshProUGUI txt_Dialogue;
    [SerializeField] TextMeshProUGUI txt_Name;

    Dialogue[] dialogues;
    bool isDialogue = false; //대화중 여부
    bool isNext = false; // 특정 키 입력 대기
    int lineCount = 0;
    int contextCount = 0;

    void Start()
    {
        windowOnOff = DialogueBar.GetComponent<OpenCloseWindow>();
    }

    void Update()
    {
        if (isDialogue && isNext && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed, proceeding to next dialogue.");
            isNext = false;
            txt_Dialogue.text = "";
            if (++contextCount < dialogues[lineCount].contexts.Length)
            {
                StartCoroutine(Writer());
            }
            else
            {
                contextCount = 0;
                if (++lineCount < dialogues.Length)
                {
                    StartCoroutine(Writer());
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        txt_Dialogue.text = "";
        txt_Name.text = "";
        dialogues = p_dialogues;
        isDialogue = true; // Set isDialogue to true when showing dialogue
        StartCoroutine(Writer());
    }

    void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;

        windowOnOff.CloseWindow();
        Debug.Log("Dialogue ended.");
    }

    IEnumerator Writer()
    {
        windowOnOff.OpenWindow();
        Debug.Log("Opening window and displaying dialogue.");

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("$", ",");
        txt_Dialogue.text = t_ReplaceText;

        if (dialogues[lineCount].name == "N")
        {
            txt_Name.text = "";
        }
        else
        {
            txt_Name.text = dialogues[lineCount].name;
        }

        isNext = true;
        Debug.Log("Dialogue displayed: " + t_ReplaceText);

        yield return null;
    }
}
