using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
	private static int turn = 0;
	private static int round = 1;
	public static string processTurn(List<CombatActor> combatants, bool inCombat) {
		Debug.Log("Processing turn "+(turn+1)+" of round "+round);

		combatants[turn].attack(combatants);
		if (combatants[turn].TryGetComponent(out Vihu enemyNow)) {
			//enemy.indicateTurn();
			//combatants[turn].attack(combatants);
			enemyNow.indicateAggro();
		}
		turn++;
		if (turn >= combatants.Count) {
			turn = 0;
			round++;
			foreach(var comb in combatants) {
				if (comb.TryGetComponent(out charControl cControl)) {
					cControl.replenishAP();
				}
			}
		}
		if (combatants[turn].TryGetComponent(out Vihu enemyNext)) {
			enemyNext.indicateTurn();
			//combatants[turn].attack(combatants);
			//enemy.indicateAggro();
		}
		if (combatants.Count <= 1) {
			inCombat = false;
			return "None";
		} else {
			return combatants[turn].name;
		}
	}
	// Predetermined target version
	public static string processTurn(List<CombatActor> combatants, bool inCombat, GameObject playerTarget) {
		if (turn == combatants.Count) {
			turn = 0;
			round++;
		}
		Debug.Log("Processing turn "+(turn+1)+" of round "+round);
		if (combatants[turn].TryGetComponent(out charControl cControl)) {
			Debug.Log("Targeted attack!");
			combatants[turn].attack(combatants, playerTarget);
		}
		else {
			combatants[turn].attack(combatants);
		}
		if (combatants[turn].TryGetComponent(out Vihu enemyNow)) {
			//enemy.indicateTurn();
			//combatants[turn].attack(combatants);
			enemyNow.indicateAggro();
		}
		turn++;
		if (turn >= combatants.Count) {
			turn = 0;
			round++;
			foreach(var comb in combatants) {
				if (comb.TryGetComponent(out charControl dControl)) {
					dControl.replenishAP();
				}
			}
		}		
		if (combatants[turn].TryGetComponent(out Vihu enemyNext)) {
			enemyNext.indicateTurn();
			//combatants[turn].attack(combatants);
			//enemy.indicateAggro();
		}
		if (combatants.Count <= 1) {
			inCombat = false;
			return "None";
		} else {
			return combatants[turn].name;
		}
	}
	public const float MAX_DISTANCE = 8;
	public static void findCombatants(List<CombatActor> combatants) {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
		Debug.Log("Found "+temp.Length+" allies");
		Vector3 playerPos = temp[0].transform.position;
		
		for(int i = 0; i < temp.Length; i++) {
			if (temp[i].TryGetComponent(out CombatActor actor)) {
				combatants.Add(actor);
			}
		}
		temp = GameObject.FindGameObjectsWithTag("EnemyFaction");
		for(int i = 0; i < temp.Length; i++) {
			if (temp[i].TryGetComponent(out Vihu actor)) {
				if (Vector3.Distance(actor.gameObject.transform.position, playerPos) < MAX_DISTANCE+2) {
					combatants.Add(actor);
					actor.SendMessage("haltMovement");
					actor.indicateAggro();
				}
			}
		}
		Debug.Log("Found "+temp.Length+" enemies");
		Debug.Log("Total count: "+combatants.Count);
		//inCombat = true;
		Debug.Log("Begin combat!");
	}

	public static int sortCombatantsFromColliders(Collider a, Collider b) {
		return a.gameObject.name.CompareTo(b.gameObject.name);
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
