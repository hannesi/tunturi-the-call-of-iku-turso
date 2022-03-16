using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueReply
{
    public string replyLine;
    public int leadsToDialogueStage;

    public DialogueReply(string replyLine, int leadsToDialogueStage) {
        this.replyLine = replyLine;
        this.leadsToDialogueStage = leadsToDialogueStage;
    }
}
