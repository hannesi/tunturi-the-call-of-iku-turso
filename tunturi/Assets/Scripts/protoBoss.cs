using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class protoBoss : Vihu
{
    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/
	
	public protoBoss() {
		attackRange = 3;
	}
	
	void closeDistance(Vector3[] moveSequence) {
		for (int i = 0; i < moveSequence.Length; i++) {
			if(stats[3] < 1) {break;}
			gameObject.transform.Translate(moveSequence[i], Space.World);
			//Debug.Log("Moving "+moveSequence[i].ToString());
			stats[3]--;
		}
	}
	
	public void actionSequence(List<CombatActor> combatants) {
		stats[3] = 5;
		GameObject player = null;
		
		int layerMask = 1 << 8;
		Collider[] nearbyColliders = Physics.OverlapSphere(gameObject.transform.position, 14, layerMask);
		Array.Sort(nearbyColliders, CombatManager.sortCombatantsFromColliders);
		Debug.Log("Looking for players...");
		foreach (var collider in nearbyColliders) {
			if (collider.gameObject.tag == "PlayerFaction") {
				player = collider.gameObject;
				Debug.Log("Found player at "+player.transform.position);
				break;
			}
		}
		if (player == null) {return;}
		Vector3[] movementSequence = calcTileDistance(transform.position - player.transform.position);
		closeDistance(movementSequence);
		if (stats[3] > 1) {attack(combatants);}
	}
	
	void randomTileBurst() {
		// Random numbers around 2-8? chosen
		// Create + sign of tiles around point
		// tiles hurt player standing on them
	}
	
	void coneBurst() {
		// Cone AOE from point of origin, n+2 tiles per iteration
		// xxxxx
		//	xxx
		//	 x
		//	 o
	}
}
