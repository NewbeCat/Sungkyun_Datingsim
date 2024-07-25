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

    [SerializeField] TextMeshProUGUI txt_Dialogue0; //without img
    [SerializeField] TextMeshProUGUI txt_Dialogue1; //with img
    private TextMeshProUGUI txt_Dialogue;

    [SerializeField] TextMeshProUGUI txt_Name;
    [SerializeField] GameObject profileimg;

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
                txt_Dialogue0.text = "";
                txt_Dialogue1.text = "";
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
            else
            {
                typeEffect.Skip();
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        txt_Dialogue0.text = "";
        txt_Dialogue1.text = "";
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

        //이름
        txt_Name.text = dialogues[lineCount].name == "N" ? "" : dialogues[lineCount].name;

        //패널 설정
        if (dialogues[lineCount].name == "N") // || dialogues[lineCount].imgname[contextCount] == "0")
        {
            txt_Dialogue = txt_Dialogue0;
            profileimg.SetActive(false);
        }
        else
        {
            txt_Dialogue = txt_Dialogue1;
            profileimg.SetActive(true);
            //이미지 설정하기  if(dialogues[lineCount].imgname[contextCount] !=""){profileimg.이미지요소  =  함수(dialogues[lineCount].imgname[contextCount]) }
        }

        //글 적기
        typeEffect.TypingNewText(txt_Dialogue, t_ReplaceText);

        yield return null;
    }

    private void HandleComplete()
    {
        //if(isSelection == true){ 선택이벤트(dialogues[lineCount].choicenum[contextCount]) }
        //if(isSkip == true){ lineCount = dialogues[lineCount].skipnum[contextCount]; }
        //else{isNext = true;}
        isNext = true;
    }
}