using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueParticipant
{
    public string name;
    public MoodTexture[] textures;
}

// unity ei ilmeisesti tue dictionaryjen veivaamista editorin puolella joten tehdaan vaikeimman kautta
[System.Serializable]
public struct MoodTexture
{
    public Mood mood;
    public Texture texture;
}
