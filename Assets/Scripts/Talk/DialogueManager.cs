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
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log($"isNext: {isNext}");
            if (isNext == true)
            {
                isNext = false;
                DialogBodyText.text = "";
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
                        EndDialogue();
                    }
                }
            }
            else
            {
                Debug.Log("Skipping");
                typeEffect.Skip();
            }
        }
    }

    // Starts the dialogue with given title and dialogue node
    public void StartDialogue(Dialogue gotnode)
    {
        // Display the dialogue UI
        ShowDialogue();
        node = gotnode;

        StartCoroutine(Writer());
    }

    private void EndDialogue()
    {
        if (node.dialogueType == DialogueType.Normal)
        {
            HideDialogue();
        }
        else if (node.dialogueType == DialogueType.ConditionBased)
        {
            StartDialogue(node.nextDialogue[node.script.condition_to_occur()]);
        }
        else
        {
            // Remove any existing response buttons
            foreach (Transform child in responseButtonContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < node.responses.Count; i++)
            {
                GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = node.responses[i].responseText;

                // Setup button to trigger SelectResponse when clicked
                buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(node.responses[i], i));
            }
        }

    }

    // Handles response selection and triggers next dialogue node
    public void SelectResponse(DialogueResponse response, int index)
    {
        node.script.action_by_choice(index);

        // Check if there's a follow-up node
        if (response.nextNode != null)
        {
            StartDialogue(response.nextNode); // Start next dialogue
        }
        else
        {
            // If no follow-up node, end the dialogue
            HideDialogue();
        }
    }

    // Hide the dialogue UI
    public void HideDialogue()
    {
        isDialogue = false;
        isNext = false;
        lineCount = 0;
        contextCount = 0;
        if (DialogueParent.activeSelf) openCloseWindow.CloseWindow();
    }

    // Show the dialogue UI
    private void ShowDialogue()
    {
        if (!DialogueParent.activeSelf) openCloseWindow.OpenWindow();
        isDialogue = true;
        isNext = false;
        lineCount = 0;
        contextCount = 0;
    }

    // Check if dialogue is currently active
    public bool IsDialogueActive()
    {
        return DialogueParent.activeSelf;
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
        Debug.Log($"isNext: {isNext}");
    }

}