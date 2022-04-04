using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Vihu : CombatActor {
	
	private Material materiaali; 
	
	private bool inCombat = false;
	//private int[] stats;
	//private string name;
	// DPA, HP, Armor, AP
	public Vihu() {
		//name = monsterName;
		stats[0] = 3;
		stats[1] = 24;
		stats[2] = 13;
		stats[3] = 5;
	}
	
	
	/*public void attack() {
		GameObject closestEnemy = base.target();
		if (closestEnemy == null) {return;}
		if (closestEnemy.TryGetComponent(out CombatActor target)) {
			Debug.Log("Isku tekee " + stats[0] + " vahinkoa kohteeseen "+attackTarget.name);
		}
	}*/
	
	void approach(GameObject target) {
		Vector3 distance = transform.position - target.transform.position;
		transform.Translate(new Vector3(distance.x,0,0));
		transform.Translate(new Vector3(0,0,distance.z));
	}
	
	private void OnTriggerEnter(Collider other) {
		if(other.tag != "PlayerFaction" || inCombat) {return;}
		Debug.Log("I see you!");
		if (other.TryGetComponent(out charControl player)) {
			inCombat = true;
			player.initCombat();
		}
	}
	
	public void indicateAggro() {
		materiaali.SetColor("_Color",Color.red);
	}
	
	public void indicateTurn() {
		materiaali.SetColor("_Color",Color.yellow);
	}
	
    void Start()
    {
		materiaali = gameObject.GetComponent<Renderer>().material; // Haetaan materiaali moodivaihdoksia varten
    }
	
}