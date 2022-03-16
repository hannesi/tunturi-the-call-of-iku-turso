using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public int speakerId;
    public string line;
    public DialogueReply[] replies;
    // public Dictionary<int,string> replies;
    //TODO: SideEffect
    //TODO: Requirement

    public DialogueLine(int speakerId, string line, DialogueReply[] replies){
        this.speakerId = speakerId;
        this.line = line;
        this.replies = replies;
    }
}
