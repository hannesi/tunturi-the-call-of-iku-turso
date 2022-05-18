using System.Collections;
using System.Collections.Generic;

public enum Mood
{
    Neutral,
    Happy,
    Sad,
    Startled,
    Angry,
    Possessed
}

[System.Serializable]
public struct DialogueLine
{
    public int speakerId;
    public string line;
    public Mood mood;
    public DialogueReply[] replies;
}
