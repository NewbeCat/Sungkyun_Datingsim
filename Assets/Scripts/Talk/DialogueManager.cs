using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // UI references
    public GameObject DialogueParent; // Main container for dialogue UI
    public TextMeshProUGUI DialogTitleText, DialogBodyText, DialogBodyText1; // Text components for title and body
    public GameObject responseButtonPrefab; // Prefab for generating response buttons
    public Transform responseButtonContainer; // Container to hold response buttons

    [SerializeField] private OpenCloseWindow openCloseWindow;
    [SerializeField] private TypeEffect typeEffect;
    public bool isDialogue = false;
    private bool isNext = false;
    private int lineCount = 0;
    private int contextCount = 0;

    private int responseCount = 0;
    private int responseCount0 = 0;

    private Dialogue node;
    private void Start()
    {
        typeEffect.CompleteTextRevealed += HandleComplete;
    }

    void OnDestroy()
    {
        typeEffect.CompleteTextRevealed -= HandleComplete;
    }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of DialogueManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isDialogue == true && Input.GetKeyDown(KeyCode.Z)) // for some reason has a problem with spacebar
        {
            if (isNext == true)
            {
                isNext = false;
                if (++contextCount < node.dialogueTalk[lineCount].talkText.Length)
                {
                    StartCoroutine(Writer());
                }
                else
                {
                    contextCount = 0;
                    if (++lineCount < node.dialogueTalk.Count)
                    {
                        StartCoroutine(Writer());
                    }
                    else
                    {
                        //Debug.Log("end");
                        EndDialogue();
                    }
                }
            }
            else
            {
                //Debug.Log("skip");
                typeEffect.Skip();
            }
        }
    }

    public void StartDialogue(Dialogue gotnode)
    {
        ShowDialogue();
        //Debug.Log(gotnode);
        node = gotnode;

        StartCoroutine(Writer());
    }

    private void EndDialogue()
    {
        isNext = false;
        lineCount = 0;
        contextCount = 0;
        //Debug.Log(node + ", " + node.isItBranch);
        if (node.isItBranch == IsItBranch.No)
        {
            node.reset();
            DestroyBtns();
            if (node.dialogueType == DialogueType.Normal)
            {
                HideDialogue();
            }
            else if (node.dialogueType == DialogueType.Condition)
            {
                StartDialogue(node.nextDialogue[node.script.condition_to_occur()]);
            }
            else
            {
                for (int i = 0; i < node.responses.Count; i++)
                {
                    GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
                    buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = node.responses[i].responseText;

                    // Setup button to trigger SelectResponse when clicked
                    int capturedIndex = i;
                    List<DialogueResponse> capturedresponse = node.responses;
                    ConditionBase capturedscript = node.script;

                    buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(capturedscript, capturedresponse[capturedIndex], capturedIndex));

                    if (node.dialogueType == DialogueType.ChoiceAll && node.responses[i].nextNode != null)
                    {
                        changeBranch(capturedresponse, true);
                        responseCount = node.responses.Count;
                        buttonObj.GetComponent<Button>().onClick.AddListener(() => ChoiceAllOptions(capturedresponse, buttonObj));
                    }
                }
            }
        }
        else
        {
            TurnOnOffBtns(true);
        }

    }
    private void changeBranch(List<DialogueResponse> children, bool yesno)
    {
        foreach (DialogueResponse child in children)
        {
            if (yesno)
            {
                child.nextNode.isItBranch = IsItBranch.Yes;
            }
            else
            {
                child.nextNode.isItBranch = IsItBranch.No;
            }
        }
    }
    public void SelectResponse(ConditionBase currentscript, DialogueResponse response, int index)
    {
        TurnOnOffBtns(false);

        if (response.nextNode != null)
        {
            StartDialogue(response.nextNode);
            currentscript.action_by_choice(index);
        }
        else
        {
            currentscript.action_by_choice(index);
            HideDialogue();
        }
    }
    public void DestroyBtns()
    {
        TurnOnOffBtns(true);
        responseCount = responseCount0 = 0;
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }
    public void TurnOnOffBtns(bool onoff)
    {
        responseButtonContainer.gameObject.SetActive(onoff);
    }
    private void ShowDialogue()
    {
        if (!DialogueParent.activeSelf) openCloseWindow.OpenWindow();
        isDialogue = true;
        isNext = false;
        lineCount = 0;
        contextCount = 0;
    }
    public void HideDialogue()
    {
        isDialogue = false;
        isNext = false;
        lineCount = 0;
        contextCount = 0;
        DestroyBtns();
        if (DialogueParent.activeSelf) openCloseWindow.CloseWindow();
    }
    IEnumerator Writer()
    {
        if (node.dialogueTalk[lineCount].speaker != null) DialogTitleText.text = node.dialogueTalk[lineCount].speaker == "N" ? "" : node.dialogueTalk[lineCount].speaker;
        typeEffect.TypingNewText(DialogBodyText, node.dialogueTalk[lineCount].talkText[contextCount]);

        yield return null;
    }
    private void HandleComplete()
    {
        isNext = true;
    }
    private void ChoiceAllOptions(List<DialogueResponse> res, GameObject buttons)
    {
        responseCount0++;
        buttons.GetComponent<Button>().interactable = false;
        if (responseCount == responseCount0)
        {
            changeBranch(res, false);
        }
    }
}