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
    public TextMeshProUGUI lineSpeakerNameOutput;
    public TextMeshProUGUI lineOutput;
    public Image speakerPortrait;

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

    public int buttonHeight = 60;
    public int fontSize = 40;
    // dialogue text colors for req check thingies
    public Color checkFailedDialogueOptionColor;
    public Color checkPassedDialogueOptionColor;
    public Color normalDialogueOptionColor;
    public Color speakerLineColor;
    public Color speakerNameColor;

    // dialogue ui sounds
    public AudioSource clickAudioSource;
    public AudioClip regularSound;
    public AudioClip successSound;
    public AudioClip failureSound;


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




    // shows a line, selection of replies etc on screen
    private void ShowLine() {
        Debug.Log("showing stage: " + dialogueStage);
        DialogueLine dl = dialogue.lines[dialogueStage];
        speakerPortrait.sprite = dialogue.participants[dl.speakerId].GetPortraitByMood(dl.mood);
        lineSpeakerNameOutput.text = $"<color=#{ColorUtility.ToHtmlStringRGB(speakerNameColor)}>{dialogue.participants[dl.speakerId].name}";
        lineOutput.text = $"<color=#{ColorUtility.ToHtmlStringRGB(speakerLineColor)}>{dl.line}";

        RemoveReplyButtons();

        ResizeReplyContainerHeight(dl.replies.Length * 30);

        for (int i = 0; i < dl.replies.Length; i++) {
            CreateDialogueButton(dl.replies[i], i);
        }
        if (dialogue.voiceEffects) GenerateDialogueVoiceQueue(dl.line);
    }

    private void ResizeReplyContainerHeight(int height) {
        RectTransform rt = repliesContainer.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }


    // Creates a dialogue button on the dialogue canvas
    private void CreateDialogueButton(DialogueReply reply, int buttonSlot) {
        bool requirementsPassed = reply.requirements.Length == 0
                                    || reply.requirements
                                                .Select(requirement => partyManager.CompareProperty(requirement))
                                                .All(response => response.Item1);
        string prefix = reply.requirements.Length != 0 ? $"[{reply.requirements[0].type} {partyManager.CompareProperty(reply.requirements[0]).Item2}/{reply.requirements[0].value}] " : "";
        string coloredPrefixString = $"<color=#{ColorUtility.ToHtmlStringRGB(requirementsPassed ? checkPassedDialogueOptionColor : checkFailedDialogueOptionColor)}>{prefix}";

        GameObject button = (GameObject)Instantiate(dialogueButtonPrefab, repliesContainer.transform);
        RectTransform buttonRT = button.GetComponent<RectTransform>();
        buttonRT.anchoredPosition = new Vector2(buttonRT.anchoredPosition.x, buttonRT.anchoredPosition.y - buttonHeight / 2 - buttonHeight * buttonSlot);
        var buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = $"{coloredPrefixString}<color=#000000> {reply.replyLine}";
        buttonText.fontSize = fontSize;
        // apply clicking sound
        button.GetComponent<Button>().onClick.AddListener(() => { 
            clickAudioSource.clip = reply.requirements.Length == 0 ? regularSound
                                  : requirementsPassed ? successSound
                                  : failureSound;
            clickAudioSource.Play();
        });
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
        // TODO: SOS: Laastari :D Pakko olla ainakin yks silence line
        clipQueue.Enqueue(silenceLines[0]);
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
        Debug.Log("TODO: lock player position while dialogue is active");
    }

    private void TerminateConversation() {
        HideCanvas();
        dialogue = null;
        Debug.Log("TODO: unlock player position while dialogue is active");
    }

    public void ShowCanvas() { dialogueCanvas.SetActive(true); }
    public void HideCanvas() { dialogueCanvas.SetActive(false); }
}
