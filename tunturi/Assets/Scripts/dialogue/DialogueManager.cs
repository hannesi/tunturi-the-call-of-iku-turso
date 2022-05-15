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
    public GameObject repliesContainer;
    public GameObject dialogueButtonPrefab;
    public TextMeshProUGUI lineSpeakerOutput;
    public TextMeshProUGUI lineOutput;
    public GameObject[] replyButtons;

    // variables for playing dialogue audio stuffs
    public AudioSource voiceAudioSource;
    public AudioClip[] voiceLines;
    public AudioClip[] silenceLines;
    public int lettersPerAudioClip = 3;
    public int extraClipsPerWord = 1;

    [Range(0, 10)]
    public int silenceFrequency = 5;
    private Queue<AudioClip> clipQueue = new Queue<AudioClip>();

    // variables for tracking dialogue stage
    // TODO: fiksumpi toteutus dialogistagen seuraamiselle
    private int dialogueStage = -1;
    private int previousDialogueState = -1;
    private Dialogue dialogue;

    private PartyManager partyManager;


    void Start() {
        partyManager = GameObject.Find("PartyManager").GetComponent<PartyManager>();
    }
    void Update() {
        // check if dialogue voice line status
        if (!voiceAudioSource.isPlaying && clipQueue.Count > 0) {
            voiceAudioSource.clip = clipQueue.Dequeue();
            voiceAudioSource.Play();
        }
        // change dialogue ui
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

        RemoveReplyButtons();

        for (int i = 0; i < dl.replies.Length; i++) {
            CreateDialogueButton(dl.replies[i], i);
        }
        if (dialogue.voiceEffects) GenerateDialogueVoiceQueue(dl.line);
    }


    // Creates a dialogue button on the dialogue canvas
    private void CreateDialogueButton(DialogueReply reply, int buttonSlot) {
        bool requirementsPassed = reply.requirements.Length == 0
                                    || reply.requirements
                                                .Select(requirement => partyManager.CompareProperty(requirement))
                                                .All(response => response.Item1);
        string prefix = reply.requirements.Length != 0 ? $"[{reply.requirements[0].type} {partyManager.CompareProperty(reply.requirements[0]).Item2}/{reply.requirements[0].value}] " : "";

        GameObject button = (GameObject)Instantiate(dialogueButtonPrefab, repliesContainer.transform);
        // button.transform.localPosition = new Vector3(0,-15 - 30 * buttonSlot,0);
        RectTransform buttonRT = button.GetComponent<RectTransform>();
        buttonRT.anchoredPosition = new Vector2(buttonRT.anchoredPosition.x, buttonRT.anchoredPosition.y - 15 - 30 * buttonSlot);
        button.GetComponentInChildren<TMP_Text>().text = prefix + reply.replyLine;
        if (requirementsPassed) {
            button.GetComponent<Button>().onClick.AddListener(() => HandleStageChange(reply));
        }
    }

    private void RemoveReplyButtons() {
        while (repliesContainer.transform.childCount > 0) {
            DestroyImmediate(repliesContainer.transform.GetChild(0).gameObject);
        }
    }

    private void HandleStageChange(DialogueReply reply) {
        foreach (PropertyModificationRequest se in reply.sideEffects)
        {
            partyManager.ModifyProperty(se);
        }
        // clear voice line queue when changing dialogue stage
        clipQueue.Clear();
        dialogueStage = reply.leadsToDialogueStage;
    }

    private void GenerateDialogueVoiceQueue(string line) {
        foreach (string s in line.Split(' '))
        {
            // alla oleva mysteerivakio 
            for (int i = 0; i < (s.Length / lettersPerAudioClip) + extraClipsPerWord; i++) {
                clipQueue.Enqueue(voiceLines[Random.Range(0, voiceLines.Length)]);
            }
            if (Random.Range(1,10) <= silenceFrequency) {
                clipQueue.Enqueue(silenceLines[Random.Range(0, silenceLines.Length)]);
            }
        }
    }

    // initiates a conversation
    public void InitConversation(Dialogue dialogue) {
        this.dialogue = dialogue;
        dialogueStage = 0;
        ShowCanvas();
    }

    public void ShowCanvas() { dialogueCanvas.SetActive(true); }
    public void HideCanvas() { dialogueCanvas.SetActive(false); }
}
