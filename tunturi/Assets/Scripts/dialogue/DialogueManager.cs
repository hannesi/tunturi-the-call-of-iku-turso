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

    private PropertyManager _propertyManager;

    void Start() {
        _propertyManager = GameObject.Find("PropertyManager").GetComponent<PropertyManager>();
    }
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
    // SOS: Spagettivaroitus, läpikulku omalla vastuulla
    private void ShowLine() {
        Debug.Log("showing stage: " + dialogueStage);
        DialogueLine dl = dialogue.lines[dialogueStage];
        lineSpeakerOutput.text = dialogue.participants[dl.speakerId];
        lineOutput.text = dl.line;
        foreach (GameObject g in replyButtons) {
            g.GetComponent<Button>().onClick.RemoveAllListeners();
            g.SetActive(false);
            // replyButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
        int nextUnusedButtonIndex = 0;
        foreach (DialogueReply reply in dl.replies) {
            if (nextUnusedButtonIndex == 4) return;             // TODO: better implementation for buttons

            // true if reply has no requirements or all requirements pass 
            bool requirementsPassed = reply.requirements.Length == 0 
                                        || reply.requirements
                                                    .Select(requirement => _propertyManager.Compare(requirement))
                                                    .All(response => response.Item1);

            string prefix = reply.requirements.Length != 0 ? $"[{reply.requirements[0].type} {_propertyManager.Compare(reply.requirements[0]).Item2}/{reply.requirements[0].value}] " : "";
            replyButtons[++nextUnusedButtonIndex].SetActive(true);
            // TODO: Elegantimpi ratkaisu? Kippaa jos viittaa suoraa AddListenerin parametrissa
            // int newStage = dl.replies[i].leadsToDialogueStage;
            // replyButtons[i].GetComponent<Button>().onClick.AddListener(() => dialogueStage = newStage);
            replyButtons[nextUnusedButtonIndex].GetComponentInChildren<TMP_Text>().text = prefix + reply.replyLine;

            if (requirementsPassed) {
                replyButtons[nextUnusedButtonIndex].GetComponent<Button>().onClick.AddListener(() => handleStageChange(reply));
            }
        }
    }

    private void handleStageChange(DialogueReply reply) {
        foreach (PropertyModificationRequest se in reply.sideEffects)
        {
            _propertyManager.Modify(se);
        }
        dialogueStage = reply.leadsToDialogueStage;
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
