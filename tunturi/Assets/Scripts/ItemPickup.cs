using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    private bool uiElementVisible;
	private Item item;
	public string text;
	public string itemName;
	public int[] increases = {0,0,0,0};
	private List<int> bonus = new List<int>();
	private List<int> refs = new List<int>();
	

    public void Interact() {
        StartCoroutine(showUiElement());
		//pickUp();
    }

    private IEnumerator showUiElement() {
        uiElementVisible = true;
        yield return new WaitForSeconds(4f);
        uiElementVisible = false;
		Debug.Log("Item was picked up");
		pickUp();
    }

    void OnGUI() {
        if (uiElementVisible) {
            GUI.Box(new Rect(10, 100, 200, 50), text);
        }
    }
	
	void pickUp() {
		for (int i = 0; i<increases.Length; i++) {
			if (increases[i] > 0) {
				bonus.Add(increases[i]);
				refs.Add(i);
			}
		}
		item = new Item(itemName, refs.Count, refs.ToArray(), bonus.ToArray());
		Debug.Log(item.announce());
		int layerMask = 1 << 8;
			Collider[] collidersInRange = Physics.OverlapSphere(gameObject.transform.position, 6, layerMask);
			foreach (var obj in collidersInRange) {
				// jos objektista loytyy IInteractablen toteuttava luokka, kutsutaan Interact() -funktiota
				//PartyMember actor = obj.GetComponent<PartyMember>();
				if (obj.TryGetComponent(out PartyMember player)) {
					player.equip(item);
					}
			}
		Destroy(gameObject);
	}
	
}
