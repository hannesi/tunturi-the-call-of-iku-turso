using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable
{
    private bool uiElementVisible;

    public void Interact() {
        Debug.Log("A meal was eaten");
        StartCoroutine(showUiElement());
    }

    private IEnumerator showUiElement() {
        uiElementVisible = true;
        yield return new WaitForSeconds(3f);
        uiElementVisible = false;
		consumeMeal();
    }

    void OnGUI() {
        if (uiElementVisible) {
            GUI.Box(new Rect(10, 100, 200, 50), "You ate a hearty meal! \n Health restored by "+12);
        }
    }
	
	void consumeMeal() {
		int layerMask = 1 << 8;
			Collider[] collidersInRange = Physics.OverlapSphere(gameObject.transform.position, 6, layerMask);
			foreach (var obj in collidersInRange) {
				// jos objektista loytyy IInteractablen toteuttava luokka, kutsutaan Interact() -funktiota
				//PartyMember actor = obj.GetComponent<PartyMember>();
				if (obj.TryGetComponent(out PartyMember player)) {
					player.setHP(player.getHP() + 12);
					}
			}
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