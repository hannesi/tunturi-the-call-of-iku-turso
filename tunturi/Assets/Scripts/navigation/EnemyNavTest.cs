using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyNavTest : MonoBehaviour {
	public Transform[] waypoints = new Transform[4];
	public Transform target;
	NavMeshAgent agent;
	
	void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(target.position);
	}
	private int currentTarget = 0;
	void Update() {
		
		if (!agent.pathPending && agent.remainingDistance < 0.5f) {
			currentTarget++;
			if (currentTarget > 3) {
				currentTarget = 0;
			}
			agent.SetDestination(waypoints[currentTarget].position);
		}
	}
	
	void haltMovement() {
		agent.isStopped = true;
	}
}