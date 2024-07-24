using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject DialogueBar;
    OpenCloseWindow windowOnOff;

    [SerializeField] TextMeshProUGUI txt_Dialogue;
    [SerializeField] TextMeshProUGUI txt_Name;
    [SerializeField] private TypeEffect typeEffect;

    Dialogue[] dialogues;
    bool isDialogue = false; //대화중 여부
    bool isNext = false; // 특정 키 입력 대기
    int lineCount = 0;
    int contextCount = 0;

    void Start()
    {
        windowOnOff = DialogueBar.GetComponent<OpenCloseWindow>();
        typeEffect.CompleteTextRevealed += HandleComplete;
    }

    void OnDestroy()
    {
        typeEffect.CompleteTextRevealed -= HandleComplete;
    }

    void Update()
    {
        if (isDialogue == true && Input.GetKeyDown(KeyCode.Space))
        {
            if (isNext == true)
            {
                isNext = false;
                txt_Dialogue.text = "";
                if (++contextCount < dialogues[lineCount].contexts.Length)
                {
                    Debug.Log("starting new writer");
                    StartCoroutine(Writer());
                }
                else
                {
                    contextCount = 0;
                    if (++lineCount < dialogues.Length)
                    {
                        Debug.Log("starting new writer of different speaker");
                        StartCoroutine(Writer());
                    }
                    else
                    {
                        EndDialogue();
                    }
                }
            }
            else
            {
                Debug.Log("Skippsies");
                typeEffect.Skip();
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        txt_Dialogue.text = "";
        txt_Name.text = "";
        dialogues = p_dialogues;
        isDialogue = true;
        StartCoroutine(Writer());
    }

    void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        EventSystem.current.SetSelectedGameObject(null); // 눌럿던 버튼에 집중하는 일을 방지 - space버튼만 생기는 에러
        windowOnOff.CloseWindow();
    }

    IEnumerator Writer()
    {
        windowOnOff.OpenWindow();

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("$", ",");
        typeEffect.TypingNewText(txt_Dialogue, t_ReplaceText);

        txt_Name.text = dialogues[lineCount].name == "N" ? "" : dialogues[lineCount].name;

        yield return null;
    }

    private void HandleComplete()
    {
        isNext = true;
        Debug.Log("Typewriter complete, isNext set to true.");
    }
}