using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    public Dialogue Dialogue;
    [SerializeField] private bool startRightNow = false;

    private void Start()
    {
        if (startRightNow) SpeakTo();
    }

    // Trigger dialogue for this actor
    public void SpeakTo()
    {
        DialogueManager.Instance.StartDialogue(Dialogue);
    }
}
