using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SideEffectType {
    Gold,
    Experience
}
[System.Serializable]
public class SideEffect
{
    public SideEffectType type;
    public int value;

    public SideEffect(SideEffectType type, int value) {
        this.type = type;
        this.value = value;
    }

    // TODO: replace this debug function with actual fucntionality
    public void resolve() {
        Debug.Log("Resolved a side effect. Type: " + type + ", value: " + value);
    }
}
