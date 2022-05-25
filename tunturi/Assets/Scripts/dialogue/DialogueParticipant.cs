using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DialogueParticipant
{
    public string name;
    public MoodTexture[] portraits;

    public Sprite GetPortraitByMood(Mood mood) {
        // TODO: implementation
        return portraits[0].portrait;
    }
}

// unity ei ilmeisesti tue dictionaryjen veivaamista editorin puolella joten tehdaan vaikeimman kautta
[System.Serializable]
public struct MoodTexture
{
    public Mood mood;
    public Sprite portrait;
}

