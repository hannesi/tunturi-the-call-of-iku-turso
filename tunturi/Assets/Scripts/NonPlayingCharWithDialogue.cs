using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayingCharWithDialogue : MonoBehaviour, IInteractable
{
    // private DialogueManager dialogueManager;

    // private DialogueLine[] dialogueLines = new DialogueLine[] {
    //     // 0
    //     new DialogueLine(
    //         0,
    //         "Can you help me find my doge?",
    //         new Dictionary<int,string>() {
    //             {1, "Of course!"},
    //             {2, "Depends on the reward..."},
    //             {-1, "I'd rather not."}
    //         }
    //     ),
    //     // 1
    //     new DialogueLine(
    //         0,
    //         "Oh thank you, kind stranger! He's brown and his name is Mr. Snotboogers and...",
    //         new Dictionary<int,string>() {
    //             {-1, "My pleasure!"}
    //         }
    //     ),
    //     // 2
    //     new DialogueLine(
    //         0,
    //         "I'll give you a silver coin for your trouble",
    //         new Dictionary<int,string>() {
    //             {1, "That's a fair reward."},
    //             {3, "My time is worth more than that..."},
    //             {-1, "Actually, I think I have something more important to do..."}
    //         }
    //     ),
    //     // 3
    //     new DialogueLine(
    //         0,
    //         "B-but that's all I have...",
    //         new Dictionary<int,string>() {
    //             {1, "No need to get upset. One silver will do."},
    //             {-1, "Pfft. Good luck finding your dog."},
    //             {0, "DEBUG: Return to the initial DialogueLine."}
    //         }
    //     ),
    // };

    void Start() {
        // dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    public void Interact() {
        // dialogueManager.InitConversation(dialogueLines);
    }
}
