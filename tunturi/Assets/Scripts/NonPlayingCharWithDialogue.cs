using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayingCharWithDialogue : MonoBehaviour, IInteractable
{
    private bool uiElementVisible;

    public void Interact() {
        print("NPC was talked to");
        StartCoroutine(showUiElement());
    }

    private IEnumerator showUiElement() {
        uiElementVisible = true;
        yield return new WaitForSeconds(3f);
        uiElementVisible = false;
    }

    void OnGUI() {
        if (uiElementVisible) {
            GUI.Box(new Rect(10, 100, 200, 50), "NPC: Hello there, adventurer!");
        }
    }
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
