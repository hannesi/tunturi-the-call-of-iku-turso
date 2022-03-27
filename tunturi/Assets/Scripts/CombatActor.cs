using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActor : MonoBehaviour {
	// DPA, HP, Armor, AP
	protected int[] stats = {0,0,0,0};
	
	public CombatActor() {
		stats[0] = 1;
	}
	/*private string name;
	public string Name{
		get{return name;}
		set{name = value;}
	}*/
	//void attack();
	protected GameObject target() {
		// Overlapsphere (consider placing combatactors in own layer to save space)
		// Choose closest target, calculate distance, move to if melee, shoot if ranged
		int layerMask = 1 << 8;
		Collider[] nearbyColliders = Physics.OverlapSphere(gameObject.transform.position, 500, layerMask);
		Debug.Log("Found "+nearbyColliders.Length+" potential targets");
		GameObject closestActor = null;
		Vector3 closestVector = new Vector3(0,0,0);
		Vector3 tempClosest;
		
		for (int i = 0; i < nearbyColliders.Length; i++) {
			if (nearbyColliders[i].gameObject != gameObject) {
				if (closestActor == null) {
					closestVector = nearbyColliders[i].gameObject.transform.position - transform.position;
					closestActor = nearbyColliders[i].gameObject;
				}
				else {
					tempClosest = nearbyColliders[i].gameObject.transform.position - transform.position;
					if ((tempClosest.x + tempClosest.z) < (closestVector.x + closestVector.z)) {
						closestVector = tempClosest;
						closestActor = nearbyColliders[i].gameObject;
					}
				}
			}
		}
		Debug.Log("Targeting...");
		return closestActor;
	}
	
	public GameObject target(int i) {
		// Overlapsphere (consider placing combatactors in own layer to save space)
		// Choose closest target, calculate distance, move to if melee, shoot if ranged
		int layerMask = 1 << 8;
		Collider[] nearbyColliders = Physics.OverlapSphere(gameObject.transform.position, 500, layerMask);
		Debug.Log("Found "+nearbyColliders.Length+" potential targets");
		GameObject targetedActor = null;
		GameObject currentTarget;
		if (gameObject.TryGetComponent(out charControl dController)) {
				currentTarget = dController.playerTarget;
				while(nearbyColliders[i].gameObject == currentTarget) {
					i++;
					if(i == nearbyColliders.Length) {
						i = 0;
						if (gameObject.TryGetComponent(out charControl cController)) {
							cController.targetCounter = 0;
						}
					}
				}
		}
		//Vector3 closestVector = new Vector3(0,0,0);
		//Vector3 tempClosest;
		if (i == nearbyColliders.Length) {
			i = 0;
			if (gameObject.TryGetComponent(out charControl cController)) {
				cController.targetCounter = 0;
			}
			
		}
		
		if (nearbyColliders[i].gameObject != gameObject) {
			//closestVector = nearbyColliders[i].gameObject.transform.position - transform.position;
			targetedActor = nearbyColliders[i].gameObject;
		}
		if (i < nearbyColliders.Length) {
			if (gameObject.TryGetComponent(out charControl cController)) {
				cController.targetCounter++;
			}
		}
		Debug.Log("i = "+i);
		if (targetedActor != null) {
		Debug.Log("Targeting "+targetedActor.name);
		}
		return targetedActor;
	}
	
	public void attack(List<CombatActor> combatants) {
		GameObject closestEnemy = target();
		if (closestEnemy == null) {
			Debug.Log("Attack failed, no nearby targets");
			return;
			}
		if (closestEnemy.TryGetComponent(out CombatActor targetActor)) {
			Debug.Log(gameObject.name + ":n isku tekee " + targetActor.name + " " + stats[0] + " vahinkoa!");
			targetActor.setHP(targetActor.stats[1] - stats[0]);
			Debug.Log(targetActor.name + " HP on nyt "+targetActor.getHP());
			if(targetActor.getHP() < 0) {
				combatants.Remove(targetActor);
				Destroy(closestEnemy);
			}			
		}
	}
	// Alternate attack to a predetermined enemy
	public void attack(List<CombatActor> combatants, GameObject targetedEnemy) {
		if (targetedEnemy == null) {
			Debug.Log("Attack failed, no nearby targets");
			return;
			}
		if (targetedEnemy.TryGetComponent(out CombatActor targetActor)) {
			Debug.Log("Isku tekee " + targetActor.name + " " + stats[0] + " vahinkoa!");
			targetActor.setHP(targetActor.stats[1] - stats[0]);
			Debug.Log(targetActor.name + " HP on nyt "+targetActor.getHP());
			if(targetActor.getHP() < 0) {
				combatants.Remove(targetActor);
				Destroy(targetedEnemy);
			}
		}
	}
	
	/*void measure() {
		
	}*/
	
	public int getDMG() {
		return stats[0];
	}
	
	public int getHP() {
		return stats[1];
	}
	
	public void setHP(int amount) {
		stats[1] = amount;
	}
	
}