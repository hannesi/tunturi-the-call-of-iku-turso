using System.Collections;
using System.Collections.Generic;

public class DialogueLine
{
    public string speaker { get; }
    public string line { get; }
    public Dictionary<int,string> replies { get; }

    public DialogueLine(string speaker, string line, Dictionary<int,string> replies){
        this.speaker = speaker;
        this.line = line;
        this.replies = replies;
    }
}
