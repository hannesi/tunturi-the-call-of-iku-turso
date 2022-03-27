using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
	private static int turn = 0;
	private static int round = 1;
	public static void processTurn(List<CombatActor> combatants, bool inCombat) {
		if (turn >= combatants.Count) {
			turn = 0;
			round++;
			foreach(var comb in combatants) {
				if (comb.TryGetComponent(out charControl cControl)) {
					cControl.replenishAP();
				}
			}
		}
		Debug.Log("Processing turn "+(turn+1)+" of round "+round);
		combatants[turn].attack(combatants);
		turn++;
		if (combatants.Count == 1) {
			inCombat = false;
			return;
		}
	}
	// Predetermined target version
	public static void processTurn(List<CombatActor> combatants, bool inCombat, GameObject playerTarget) {
		if (combatants.Count == 0) {
			inCombat = false;
			return;
			}
		if (turn == combatants.Count) {
			turn = 0;
			round++;
		}
		Debug.Log("Processing turn "+(turn+1)+" of round "+round);
		if (combatants[turn].TryGetComponent(out charControl cControl)) {
			Debug.Log("Targted attack!");
			combatants[turn].attack(combatants, playerTarget);
		}
		else {
			combatants[turn].attack(combatants);
		}
		turn++;
	}
	
	public static void findCombatants(List<CombatActor> combatants) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
		Debug.Log("Found "+temp.Length+" allies");
		Vector3 playerPos = temp[0].transform.position;
		float maxDistance = 12;
		for(int i = 0; i < temp.Length; i++) {
			if (temp[i].TryGetComponent(out CombatActor actor)) {
				combatants.Add(actor);
			}
		}
		temp = GameObject.FindGameObjectsWithTag("EnemyFaction");
		for(int i = 0; i < temp.Length; i++) {
			if (temp[i].TryGetComponent(out CombatActor actor)) {
				if (Vector3.Distance(actor.gameObject.transform.position, playerPos) < maxDistance) {
					combatants.Add(actor);
				}
			}
		}
		Debug.Log("Found "+temp.Length+" enemies");
		Debug.Log("Total count: "+combatants.Count);
		//inCombat = true;
		Debug.Log("Begin combat!");
	}
	
	/*
	void fight(CombatActor[] combatants) {
		for(int i = 0; i < combatants.Length; i++) {
			if(combatants[i].getHP() <= 0) {continue;}
			combatants[i].attack();
		}
	}*/
	
	// Every time the UI-button is pressed, a turn is processed
}
