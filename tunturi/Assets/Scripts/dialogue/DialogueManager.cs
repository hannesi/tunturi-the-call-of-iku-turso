using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Unity objects
    public GameObject dialogueCanvas;
    public TextMeshProUGUI lineSpeakerOutput;
    public TextMeshProUGUI lineOutput;
    public TextMeshProUGUI replySpeakerOutput;
    public GameObject[] replyButtons;

    // variables for tracking dialogue stage
    // TODO: fiksumpi toteutus dialogistagen seuraamiselle
    private int dialogueStage = -1;
    private int previousDialogueState = -1;
    private Dialogue dialogue;

    void Update() {
        if (dialogueStage != previousDialogueState) {
            if (dialogueStage == -1) {
                TerminateConversation();
                Debug.Log("Dialogue terminated");
            } else {
                ShowLine();
                Debug.Log("Dialogue stage changed: " + dialogueStage);
            }
        }
        previousDialogueState = dialogueStage;
    }


    private void TerminateConversation() {
        HideCanvas();
        dialogue = null;
    }

    // shows a line, selection of replies etc on screen
    private void ShowLine() {
        Debug.Log("showing stage: " + dialogueStage);
        DialogueLine dl = dialogue.lines[dialogueStage];
        lineSpeakerOutput.text = dialogue.participants[dl.speakerId];
        lineOutput.text = dl.line;
        for (int i = 0; i < replyButtons.Length; i++) {
            if (i < dl.replies.Length) {
                replyButtons[i].SetActive(true);
                // TODO: Elegantimpi ratkaisu? Kippaa jos viittaa suoraa AddListenerin parametrissa
                int newStage = dl.replies[i].leadsToDialogueStage;
                replyButtons[i].GetComponent<Button>().onClick.AddListener(() => dialogueStage = newStage);
                replyButtons[i].GetComponentInChildren<TMP_Text>().text = dl.replies[i].replyLine;
            } else {
                replyButtons[i].SetActive(false);
            }
        }
    }

    // initiates a conversation
    public void InitConversation(Dialogue dialogue) {
        this.dialogue = dialogue;
        dialogueStage = 0;
        ShowCanvas();
    }

    // SIGKILL current conversation
    public void TerminateCurrentConversation() {
        // TODO: toteutukseen tulee liittymaan tuhat exploittia
        dialogueStage = -1;
    }
    public void ShowCanvas() { dialogueCanvas.SetActive(true); }
    public void HideCanvas() { dialogueCanvas.SetActive(false); }
}
