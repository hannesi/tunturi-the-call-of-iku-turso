using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct DialogueReply
{
    public string replyLine;
    public int leadsToDialogueStage;
    public PropertyModificationRequest[] sideEffects;
    public PropertyComparisonQuery[] requirements;

}
