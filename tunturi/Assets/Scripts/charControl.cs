using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class charControl : MonoBehaviour
{
	private CharacterController controller;
	private Vector3 pVelocity;
	private Vector3 pPosition;
	private bool hasJumped = true;
	private bool turnBased = false;
	private bool inCombat = false;
	private Material materiaali; 
	public float moveSpeed = 3.0f;
	public float gravity = 9.81f;
	public int maxMoves;
	public float interactionRange = 1.0f;
	
    // Start is called before the first frame update
    void Start()
    {
		materiaali = gameObject.GetComponent<Renderer>().material; // Haetaan ohjattavan materiaali moodivaihdoksia varten
        controller = gameObject.GetComponent("CharacterController") as CharacterController;
    }
	public int targetCounter = 0;
	public GameObject playerTarget = null;
    // Update is called once per frame
    void Update()
    {
		// The designated button changes the movement mode of the player by flipping the current mode
		if (Input.GetButtonDown("Fire3") && !inCombat) {
			flipTurnbased();
			pPosition = transform.position; // Position is aligned to nearest grid square
			transform.position = pPosition - new Vector3( (pPosition.x % 1) , 0 , (pPosition.y % 1) );
			// TODO Add logic (Raycast?) to determine if space is empty
		}
        float verticalInput = Input.GetAxis("Vertical");
		float horizontalInput = Input.GetAxis("Horizontal");
		if (turnBased)  // Turn-based movement logic - Once the amount of moves has been used, controls are locked
		{	// TODO tutki Raycast, josko tehokkaasti tarkastaisi onko ruutu vapaa
			if(materiaali.GetColor("_Color") != Color.green && materiaali.GetColor("_Color") != Color.red) {
				materiaali.SetColor("_Color",Color.green);
			}
			if (maxMoves > 0) {
				if (Input.GetKeyDown("d") && noCollisions(Vector3.right)) {
					transform.Translate(Vector3.right);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
				}
				else if (Input.GetKeyDown("s") && noCollisions(Vector3.back)) {
					transform.Translate(Vector3.back);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
				}
				else if (Input.GetKeyDown("a") && noCollisions(Vector3.left)) {
					transform.Translate(Vector3.left);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
				}
				else if (Input.GetKeyDown("w") && noCollisions(Vector3.forward)) {
					transform.Translate(Vector3.forward);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
				} // A visual indicator for 0 moves
				if (maxMoves == 0) { 
					materiaali.SetColor("_Color",Color.red);
				}
			}
			if (inCombat && Input.GetKeyDown("r")) {
				if (playerTarget != null) {
				CombatManager.processTurn(combatants, inCombat, playerTarget);					
				}
				else {
					CombatManager.processTurn(combatants, inCombat);
				}
			}
			if (inCombat && Input.GetKeyDown("f")) {
				if (gameObject.TryGetComponent(out CombatActor playerActor)) {
					playerTarget = playerActor.target(targetCounter);
				}
			}

		}
		else // The logic of free, unrestricted movement. Includes jumping.
		{
			if(materiaali.GetColor("_Color") != Color.white) {
				materiaali.SetColor("_Color",Color.white);
			}
			
			Vector3 mvmnt = new Vector3(horizontalInput, 0 ,verticalInput);
			controller.Move(mvmnt * moveSpeed * Time.deltaTime);
			if (controller.isGrounded && pVelocity.y < 0) {
				pVelocity.y = 0;
				hasJumped = false;
		} 		
		if (pVelocity.y == 0) {
			hasJumped = false;
		}
		if(Input.GetButtonDown("Jump") && !(hasJumped)) {
			pVelocity.y = 10.00f;
			hasJumped = true;
			controller.Move(pVelocity * Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			// kerataan rangessa olevat gameobjektit taulukkoon
			Collider[] collidersInRange = Physics.OverlapSphere(gameObject.transform.position, interactionRange);
			// kaydaan kaikki keratyt gameobjektit lapi
			foreach (var obj in collidersInRange) {
				// jos objektista loytyy IInteractablen toteuttava luokka, kutsutaan Interact() -funktiota
				IInteractable interactableObj = obj.GetComponent<IInteractable>();
				if (interactableObj != null) {
					interactableObj.Interact();
				}
				// if (obj is IInteractable interactableObj) {
				// 	interactableObj.Interact();
				// }
			}
		}

		pVelocity.y -= (gravity * Time.deltaTime);
		
		controller.Move(pVelocity * Time.deltaTime);
		}
		if(combatants.Count == 1 && inCombat) {
			inCombat = false;
			flipTurnbased();
		}			
		
    }
	// Universal function for flipping the turn-based mode
	private void flipTurnbased() {
		if (turnBased) { // Free-play mode is engaged
			turnBased = false;
		}
		else { // Turn-based mode is engaged, the amount of moves a player has
			turnBased = true;
			maxMoves = 5;
			Debug.Log("Moves remaining: " + maxMoves);
		}
	}
	
	public List<CombatActor> combatants = new List<CombatActor>();
	// Called when combat starts
	public void initCombat() {
		if(!turnBased) {flipTurnbased();}
		CombatManager.findCombatants(combatants);
		inCombat = true;
		/*GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayerFaction");
		Debug.Log("Found "+temp.Length+" allies");
		for(int i = 0; i < temp.Length; i++) {
			if (temp[i].TryGetComponent(out CombatActor actor)) {
				combatants.Add(actor);
			}
		}
		temp = GameObject.FindGameObjectsWithTag("EnemyFaction");
		for(int i = 0; i < temp.Length; i++) {
			if (temp[i].TryGetComponent(out CombatActor actor)) {
				combatants.Add(actor);
			}
		}
		Debug.Log("Found "+temp.Length+" enemies");
		Debug.Log("Total count: "+combatants.Count);
		inCombat = true;
		Debug.Log("Begin combat!");*/
	}
	
	/*private int turn = 0;
	private int round = 1;
	void processTurn() {
		if (combatants.Count == 0) {
			inCombat = false;
			return;
			}
		if (turn == combatants.Count) {
			turn = 0;
			round++;
		}
		Debug.Log("Processing turn "+(turn+1)+" of round "+round);
		combatants[turn].attack();
		turn++;
	}*/
	
	// Function for checking for possible collisions during TB-movement
	// TODO: add layer masking for object colliders
	private bool noCollisions(Vector3 offset) {
		if (Physics.Linecast(transform.position, (transform.position + offset), Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
			return false;
		}
		else {
			return true;
		}
	}
	
	// Default text of the button
	public string modeButtonText = "Free-roam";
	// GUI-elements for HUD, current implementation contains a button for movement mode which reactively changes its' text
	void OnGUI() {
		if (GUI.Button (new Rect(0, Screen.height-150, 100, 50), modeButtonText) && !inCombat) {
			if (turnBased) {
			//turnBased = false;
				flipTurnbased();
				modeButtonText = "Free-roam";
				//Debug.Log("HÄH");
			}
			else {
				//turnBased = true;
				flipTurnbased();
				modeButtonText = "Turn-based";
			}
		}
    }

}
