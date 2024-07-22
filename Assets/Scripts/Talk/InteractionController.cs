using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] GameObject scriptholder;

    [SerializeField] GameObject profileDM;
    [SerializeField] GameObject standDM;
    DialogueManager profileD;
    DialogueManager standingD;

    void Start()
    {
        profileD = profileDM.GetComponent<DialogueManager>();
        standingD = standDM.GetComponent<DialogueManager>();
    }

    public void Prompt()
    {
        profileD.ShowDialogue(scriptholder.GetComponent<InteractionEvent>().GetDialogue());
    }

    void interact() { }
}
