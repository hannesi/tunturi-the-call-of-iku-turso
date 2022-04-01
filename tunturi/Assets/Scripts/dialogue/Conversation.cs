using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour, IInteractable
{
    private DialogueManager dialogueManager;

    public Dialogue dialogue;

    void Start() {
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    public void Interact() {
        dialogueManager.InitConversation(dialogue);
    }
}
