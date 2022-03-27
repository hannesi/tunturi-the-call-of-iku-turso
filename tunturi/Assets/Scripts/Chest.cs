using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private bool uiElementVisible;

    public void Interact() {
        print("A container was opened");
        StartCoroutine(showUiElement());
    }

    private IEnumerator showUiElement() {
        uiElementVisible = true;
        yield return new WaitForSeconds(3f);
        uiElementVisible = false;
    }
	bool unlooted = true;
    void OnGUI() {
        if (uiElementVisible && unlooted) {
            GUI.Box(new Rect(10, 100, 200, 50), "You discover a pair of Ogre Gauntlets! \n Strength increased by 2!");
			//unlooted = false;
			updateStrength();
        } else if (uiElementVisible) {
			GUI.Box(new Rect(10, 100, 200, 50), "The chest is empty.");
		}
    }
	void updateStrength() {
			int layerMask = 1 << 8;
			Collider[] collidersInRange = Physics.OverlapSphere(gameObject.transform.position, 1, layerMask);
			foreach (var obj in collidersInRange) {
				// jos objektista loytyy IInteractablen toteuttava luokka, kutsutaan Interact() -funktiota
				//PartyMember actor = obj.GetComponent<PartyMember>();
				if (obj.TryGetComponent(out PartyMember player)) {
					player.adjustARM(2);
					player.setDMG(player.getDMG() + 2);
					}
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
