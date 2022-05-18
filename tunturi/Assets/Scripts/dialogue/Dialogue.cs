using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string[] participants;
    public DialogueLine[] lines;
    public bool voiceEffects;
    public Texture speakerAvatar;
}
