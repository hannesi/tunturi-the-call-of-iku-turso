using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActor : MonoBehaviour {
	// DPA, HP, Armor, AP
	private bool isDodging = false;
	private AudioSource tempSource;
	protected float attackRange = 0;
	public int[] stats = {0,0,0,0};
	private int baseMissChance = 33;
	private int maxHealth;
	
	public AudioClip attackSound;
	public AudioClip dodgeSound;
	//public AudioClip missSound;
	
	public CombatActor() {
		stats[0] = 1;
	}
	
	void Start() {
		tempSource = gameObject.AddComponent<AudioSource>();
		tempSource.volume = 0.3f;
		maxHealth = stats[1];
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
		Collider[] nearbyColliders = Physics.OverlapSphere(gameObject.transform.position, CombatManager.MAX_DISTANCE, layerMask);
		Array.Sort(nearbyColliders, CombatManager.sortCombatantsFromColliders);
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
		Collider[] nearbyColliders = Physics.OverlapSphere(gameObject.transform.position, CombatManager.MAX_DISTANCE, layerMask);
		Array.Sort(nearbyColliders, CombatManager.sortCombatantsFromColliders);
		Debug.Log("Found "+nearbyColliders.Length+" potential targets");
		GameObject targetedActor = null;
		//GameObject currentTarget = null;
		if (gameObject.TryGetComponent(out charControl dController)) {
				if (i >= nearbyColliders.Length) {
					dController.targetCounter = 0;
					i = 0;
					targetedActor = nearbyColliders[i].gameObject;
					dController.targetCounter++;
				} else if (dController.playerTarget == nearbyColliders[i].gameObject) {
					int k = i;
					bool breakFlag = false;
					while (nearbyColliders[k].gameObject == dController.playerTarget && !breakFlag) {
						//Debug.Log("k="+k+" ja nearbyColliders "+nearbyColliders.Length);
						if (k >= nearbyColliders.Length-1) { // Checker
							Debug.Log("breaking");
							k = 0;
							breakFlag = true;
						} else {
							k++;
						}
					}
					i = k;
					dController.targetCounter = i;
					targetedActor = nearbyColliders[i].gameObject;
					dController.targetCounter++;
					
				}
				else { 
					targetedActor = nearbyColliders[i].gameObject;
					dController.targetCounter++;
				}
		}

		if (targetedActor != null) {
		Debug.Log("i = "+i);
		Debug.Log("Targeting "+targetedActor.name);
		dController.updateTarget(targetedActor);
		return targetedActor;
		}
		else {
			return dController.playerTarget;
		}
		
	}
	public void attack(List<CombatActor> combatants) {
		GameObject closestEnemy = target();
		float maxRange = 8; // Set default, can be overriden if set on object creation
		if (attackRange != 0) {
			maxRange = attackRange;
			Debug.Log("Setting new range...");
		}
		if (closestEnemy != null) {
			if (Vector3.Distance(closestEnemy.transform.position, gameObject.transform.position) > maxRange ) {
				
				// Attack is logged to GUI combat log				
				GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
				foreach(var play in temp) {
					if(play.TryGetComponent(out charControl cControl)) {
						cControl.logAction(name+"'s attack failed, no nearby targets");
					}
				}				
				
				Debug.Log("Attack failed, no nearby targets");
				return;
				}
			if (closestEnemy.TryGetComponent(out CombatActor targetActor)) {
				if(!targetActor.isDodging && UnityEngine.Random.Range(0,100) < baseMissChance) {
					//SendMessage("logAction", name+"'s attack missed!");
					sendLogMessage(name+"'s attack missed!");
					if (dodgeSound != null && tempSource != null) {
						tempSource.clip = dodgeSound;
						tempSource.Play();
					}
					return;
				} else if (targetActor.isDodging && UnityEngine.Random.Range(0,100) < (baseMissChance+27))  {
					//SendMessage("logAction", targetActor.name+" dodges the attack!");
					sendLogMessage(name+" dodges the attack!");
					if (dodgeSound != null && tempSource != null) {
						tempSource.clip = dodgeSound;
						tempSource.Play();
					}
					return;
				}
				// Attack is logged to GUI combat log				
				GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
				foreach(var play in temp) {
					if(play.TryGetComponent(out charControl cControl)) {
						cControl.logAction(name+" strikes " + targetActor.name + " for " + (stats[0] - targetActor.stats[2]) + " damage! \n "+targetActor.name+" has "+(targetActor.getHP()-stats[0])+"HP");
					}
				}				
				if (tempSource != null && attackSound != null) { // Creates a temporary source to play attack sound, if it exists
						tempSource.clip = attackSound;
						tempSource.Play();
				}
				Debug.Log(gameObject.name + " strikes " + targetActor.name + " and deals " + (stats[0] - targetActor.stats[2]) + " damage!");
				targetActor.setHP(targetActor.stats[1] - (stats[0] - targetActor.stats[2])); // Armor acts as flat dmg reduction
				Debug.Log(targetActor.name + " HP is now "+targetActor.getHP());
				if(targetActor.getHP() <= 0) {
					combatants.Remove(targetActor);
					if (targetActor.tag != "PlayerFaction") {Destroy(closestEnemy);}
				}			
			}
		} else {
			// Attack is logged to GUI combat log				
			GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
			foreach(var play in temp) {
				if(play.TryGetComponent(out charControl cControl)) {
					cControl.logAction(name+"'s attack failed, no nearby targets");
				}
			}				
			Debug.Log(name+"'s attack failed, no nearby targets");
		}
	}
	// Method for sending messages to player log, for use in both enemies and friendlies
	void sendLogMessage(string message) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
		foreach(var play in temp) {
			if(play.TryGetComponent(out charControl cControl)) {
				cControl.logAction(message);
			}
		}		
	}
	
	
	// Alternate attack to a predetermined enemy
	public void attack(List<CombatActor> combatants, GameObject targetedEnemy) {
		float maxRange = 12;
		if (targetedEnemy != null) {
			if (Vector3.Distance(targetedEnemy.transform.position, gameObject.transform.position) > maxRange) {
				
				// Attack is logged to GUI combat log				
				GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
				foreach(var play in temp) {
					if(play.TryGetComponent(out charControl cControl)) {
						cControl.logAction(name+"'s attack failed, no nearby targets");
					}
				}					
				
				
				Debug.Log("Attack failed, no nearby targets");
				return;
				}
			if (targetedEnemy.TryGetComponent(out CombatActor targetActor)) {
				// Attack is logged to GUI combat log				
				if(!targetActor.isDodging && UnityEngine.Random.Range(0,100) < (baseMissChance-16)) {
					//SendMessage("logAction", name+"'s attack missed!");
					sendLogMessage(name+"'s attack missed!");
					if (dodgeSound != null && tempSource != null) {
						tempSource.clip = dodgeSound;
						tempSource.Play();
					}
					return;
				} else if (targetActor.isDodging && UnityEngine.Random.Range(0,100) < baseMissChance)  {
					//SendMessage("logAction", targetActor.name+" dodges the attack!");
					sendLogMessage(name+"dodges the attack!");
					if (dodgeSound != null && tempSource != null) {
						tempSource.clip = dodgeSound;
						tempSource.Play();
					}
					return;
				}
				GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
				foreach(var play in temp) {
					if(play.TryGetComponent(out charControl cControl)) {
						cControl.logAction(name+" strikes " + targetActor.name + " for " + stats[0] + " damage! \n "+targetActor.name+" has "+(targetActor.getHP()-stats[0])+"HP");
					}
					if (tempSource != null && attackSound != null) { // Creates a temporary source to play attack sound, if it exists
						tempSource.clip = attackSound;
						tempSource.Play();
					}
				}
				
				Debug.Log(name+" strikes " + targetActor.name + " for " + stats[0] + " damage!");
				targetActor.setHP(targetActor.stats[1] - stats[0]);
				Debug.Log(targetActor.name + " HP on nyt "+targetActor.getHP());
				if(targetActor.getHP() <= 0) {
					combatants.Remove(targetActor);
					Destroy(targetedEnemy);
					//GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
					foreach(var play in temp) {
						if(play.TryGetComponent(out charControl cControl)) {
							cControl.hideIndicator();
						}
					}
				}
			}
		} else {
			// Attack is logged to GUI combat log				
			GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
			foreach(var play in temp) {
				if(play.TryGetComponent(out charControl cControl)) {
					cControl.logAction(name+"'s attack failed, no nearby targets");
				}
			}				
			Debug.Log(name+"'s attack failed, no nearby targets");
		}
	}
	
	/*void measure() {
		
	}*/
	// Encapsulated functions for accessing dodge, used in CombatManager to reset dodge state
	public void dodge() {
		isDodging = true;
	}
	public void unDodge() {
		isDodging = false;
	}
	public bool dodgeState() {
		return isDodging;
	}
	
	public int getDMG() {
		return stats[0];
	}
	
	public void setDMG(int amount) {
		stats[0] = amount;
	}
	
	public int getHP() {
		return stats[1];
	}
	public int getHPMax() {
		return maxHealth;
	}
	
	public void setHP(int amount) {
		stats[1] = amount;
	}
	
}