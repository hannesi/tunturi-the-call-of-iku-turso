using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct DialogueLine
{
    public int speakerId;
    public string line;
    public DialogueReply[] replies;
}
