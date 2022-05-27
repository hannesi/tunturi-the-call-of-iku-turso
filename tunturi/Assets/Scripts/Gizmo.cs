using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour, IInteractable
{
    private bool uiElementVisible;

    public void Interact() {
        Debug.Log("Player acquired gizmo");
        StartCoroutine(showUiElement());
    }

    private IEnumerator showUiElement() {
        uiElementVisible = true;
        yield return new WaitForSeconds(9f);
        uiElementVisible = false;
		takeGizmo();
    }

    void OnGUI() {
        if (uiElementVisible) {
            GUI.Box(new Rect(10, 100, 200, 50), "You discover a strange gizmo! \n But this is only a demo... \n Thank you for playing!");
        }
    }
	
	void takeGizmo() {
		Destroy(gameObject);
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