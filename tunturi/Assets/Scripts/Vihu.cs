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
		stats = new int[]{6, 24, 0, 5};
		/*stats[0] = 3;
		stats[1] = 24;
		stats[2] = 13;
		stats[3] = 5;*/
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
			//SendMessage("haltMovement");
			inCombat = true;
			player.initCombat();
		}
	}
	
	/*public void coneAttack() {
		for(int i = 0; i<5; i++) {
			
		}
	}*/
	private GameObject closestPlayer;
	public void findClosestPlayer() {
		GameObject[] nearPlayers = GameObject.FindGameObjectsWithTag("PlayerFaction");
		if (nearPlayers.Length>0)  {
			closestPlayer = nearPlayers[0];
		}
		for (int i = 0; i<nearPlayers.Length; i++) {
			if(Vector3.Distance(transform.position, nearPlayers[i].transform.position) < Vector3.Distance(transform.position, closestPlayer.transform.position) ) {
				closestPlayer = nearPlayers[i];
			}
		}
	}
	// Calculates distance in tiles to player by providing array of singular vectors
	public static Vector3[] calcTileDistance(Vector3 dist) {
		int oneones = 0;
		int onezeros = 0;
		int zeroones = 0;
		//bool xneg = false;
		//bool zneg = false;
		
		List<Vector3> singulars = new List<Vector3>();
		
		int xround = (int)Math.Round(dist.x);
		int zround = (int)Math.Round(dist.z);
		int xabsround = Math.Abs(xround);
		int zabsround = Math.Abs(zround);
		Vector3 offsetVector;
		Vector3 singu = Vector3.zero;
		// Exception for possible zeroes to prevent zero-division errors
		if ( (xround != 0) && (zround != 0) ) {
			singu = new Vector3( (xround / xabsround ),0,(zround / zabsround ) ); // Produces singular oneone vector
			singu = (Vector3.zero - singu); // We reverse the singular vector
		}
		if (xabsround > zabsround) {
			oneones = zabsround; // We obtain the amount of singular vectors needed
			if (xround > 0) { // We obtain the remaining offset from the distance given
				offsetVector = Vector3.left;
			} else {
				offsetVector = Vector3.right;
			}
			onezeros = xabsround - zabsround;
		} else if (xabsround < zabsround) {
			oneones = xabsround;
			if (zround > 0) {
				offsetVector = Vector3.back;
			} else {
				offsetVector = Vector3.forward;
			}
			zeroones = zabsround - xabsround;
		} else {
			oneones = xabsround;
			offsetVector = Vector3.zero;
		}
		// Based on amounts calculated above, the vectors are added onto the list
		for (int i = 0; i<oneones;i++) {
			if (singu != Vector3.zero) {
				singulars.Add(singu);
			}
		}
		for (int i = 0; i<onezeros;i++) {
			singulars.Add(offsetVector);
		}
		for (int i = 0; i<zeroones;i++) {
			singulars.Add(offsetVector);
		}
		// List is converted on an array for storage purposes
		if (singulars.Count > 0) {singulars.RemoveAt(singulars.Count-1);}
		return singulars.ToArray();
		
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