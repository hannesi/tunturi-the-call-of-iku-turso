using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Goblet : MonoBehaviour, IInteractable
{
    private bool uiElementVisible;

    public void Interact() {
        Debug.Log("Test ended");
        StartCoroutine(showUiElement());
    }

    private IEnumerator showUiElement() {
        uiElementVisible = true;
        yield return new WaitForSeconds(3f);
        uiElementVisible = false;
    }

    void OnGUI() {
        if (uiElementVisible) {
            GUI.Box(new Rect(10, 100, 600, 50), "You win!");
			EditorApplication.ExitPlaymode();
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
