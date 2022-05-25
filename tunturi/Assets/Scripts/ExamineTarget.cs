using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineTarget : MonoBehaviour, IInteractable
{
    private bool uiElementVisible;
	public string examineText  = "bottom text";

    public void Interact() {
        print("Item examined");
        StartCoroutine(showUiElement());
    }

    private IEnumerator showUiElement() {
        uiElementVisible = true;
        yield return new WaitForSeconds(6f);
        uiElementVisible = false;
    }

    void OnGUI() {
		float heightFloat = (float)(Screen.height*0.1);
        if (uiElementVisible) {
            GUI.Box(new Rect(10, heightFloat, Screen.width/3, 50), examineText);
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