using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour, IInteractable
{
	
    private bool uiElementVisible;

    public void Interact() {
		boulderEvaluate();
        print("NPC was talked to");
        StartCoroutine(showUiElement());
    }

    private IEnumerator showUiElement() {
        uiElementVisible = true;
        yield return new WaitForSeconds(3f);
        uiElementVisible = false;
    }
	bool worthy;
    void OnGUI() {
        if (uiElementVisible && worthy) {
            GUI.Box(new Rect(10, 100, 600, 50), "You move the boulder with your mighty strength!");
			Destroy(gameObject);
        }
		else if (uiElementVisible) {
			GUI.Box(new Rect(10, 100, 600, 50), "You are too weak to move the boulder! \n Perhaps you may improve your strength somehow?");
		}
    }
	
	void boulderEvaluate() {
			int layerMask = 1 << 8;
			Collider[] collidersInRange = Physics.OverlapSphere(gameObject.transform.position, 4, layerMask);
			// kaydaan kaikki keratyt gameobjektit lapi
			foreach (var obj in collidersInRange) {
				// jos objektista loytyy IInteractablen toteuttava luokka, kutsutaan Interact() -funktiota
				//PartyMember actor = obj.GetComponent<PartyMember>();
				/*if (actor.getARM() > 6) {
					worthy = true;
					Debug.Log("STR is "+actor.getARM());
				}
				else {
					worthy = false;
					Debug.Log("STR is "+actor.getARM());
				}*/
				if (obj.TryGetComponent(out PartyMember actor)) {
					//Debug.Log("man found");
					if (actor.getARM() > 6) {
						worthy = true;
						Debug.Log("STR is "+actor.getARM());
					}
					else {
						worthy = false;
						Debug.Log("STR is "+actor.getARM());
					}
				}
				// if (obj is IInteractable interactableObj) {
				// 	interactableObj.Interact();
				// }
			}
	}
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
