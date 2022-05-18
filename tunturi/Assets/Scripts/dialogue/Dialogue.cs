using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public DialogueParticipant[] participants;
    public DialogueLine[] lines;
    public bool voiceEffects;
}

