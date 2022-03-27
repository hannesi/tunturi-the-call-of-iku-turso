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
            GUI.Box(new Rect(10, 100, 600, 50), "Recover the Sacred Goblet from the killer Cylinders! \n You recall the rune R progresses turns, and the rune F changes targets.");
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
