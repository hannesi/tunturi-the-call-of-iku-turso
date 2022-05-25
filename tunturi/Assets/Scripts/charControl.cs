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
	private GameObject targetIndicator;
	private GameObject grid;
	private AudioManager audioManager;
	
	public float moveSpeed = 3.0f;
	public float gravity = 9.81f;
	public int maxMoves;
	public float interactionRange = 1.0f;
	
    // Start is called before the first frame update
	// TODO: create grid and tile for instantiation
    void Start()
    {
		
		grid = GameObject.Find("baseGrid");
		targetIndicator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		targetIndicator.transform.position = new Vector3(0,-3,0);
		targetIndicator.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
		materiaali = gameObject.GetComponent<Renderer>().material; // Haetaan ohjattavan materiaali moodivaihdoksia varten
        controller = gameObject.GetComponent("CharacterController") as CharacterController;
		
		mainCamera = gameObject.GetComponentInChildren<Camera>();
		
		audioManager = GetComponent<AudioManager>();
    }
	
	private Vector3 tempVector; //Temporary vector for storing movement in TB-mode
	private Camera mainCamera;
	
	public int targetCounter = 0;
	public GameObject playerTarget = null;
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown("j")) {
			mainCamera.transform.RotateAround(transform.position, Vector3.up, 45);
			/*Vector3 tempVec = new Vector3(-107,0,-32);
			Vector3[] test = Vihu.calcTileDistance(tempVec);
			for (int i  = 0; i<test.Length; i++) {
				Debug.Log(test[i].ToString());
			}*/
		}
		if (Input.GetKeyDown("l")) {
			mainCamera.transform.RotateAround(transform.position, Vector3.up, -45);
		}
		if (!inCombat && !turnBased) {
			grid.transform.position = (Vector3.zero);
		}
		// The designated button changes the movement mode of the player by flipping the current mode
		if (Input.GetButtonDown("Fire3") && !inCombat && !hasJumped) {
			flipTurnbased();
			pPosition = transform.position; // Position is aligned to nearest grid square
			transform.position = pPosition - new Vector3( (pPosition.x % 1) , 0 , (pPosition.y % 1) );
			//GameObject grid = GameObject.Find("baseGrid");
			if (turnBased) {
			grid.transform.position = transform.position;
			grid.transform.Translate(new Vector3 (0,-0.3f, 0));
			} else {
				grid.transform.position = new Vector3(0,0,0);
			}
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
				if(materiaali.GetColor("_Color") != Color.green) {
					materiaali.SetColor("_Color",Color.green);
				}
				if (Input.GetKeyDown("d") && noCollisions(Vector3.right)) {
					//transform.Translate(Vector3.right);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
					tempVector += Vector3.right;
					placeTile(Vector3.right);
				}
				else if (Input.GetKeyDown("s") && noCollisions(Vector3.back)) {
					//transform.Translate(Vector3.back);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
					tempVector += Vector3.back;
					placeTile(Vector3.back);
				}
				else if (Input.GetKeyDown("a") && noCollisions(Vector3.left)) {
					//transform.Translate(Vector3.left);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
					tempVector += Vector3.left;
					placeTile(Vector3.left);
				}
				else if (Input.GetKeyDown("w") && noCollisions(Vector3.forward)) {
					//transform.Translate(Vector3.forward);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
					tempVector += Vector3.forward;
					placeTile(Vector3.forward);
				} 
			}
			if (inCombat && Input.GetKeyDown("q") && (maxMoves > 1) ) {
				
				if (gameObject.TryGetComponent(out PartyMember controlledMember)) {
					if (playerTarget != null) {
						controlledMember.attack(combatants, playerTarget);
						maxMoves -= 2;
					} else {
						controlledMember.attack(combatants);
						maxMoves--;
					}	
				}
			}
			if (inCombat && Input.GetKeyDown("z") && (maxMoves > 0) ) {
				if (gameObject.TryGetComponent(out PartyMember controlledMember)) {
					if (!controlledMember.dodgeState()) {
						controlledMember.dodge();
						maxMoves--;
						logAction(name+" is now dodging until next turn");
					}
				}
			}
			// A visual indicator for 0 moves
			if (maxMoves == 0) { 
					materiaali.SetColor("_Color",Color.red);
			}
			if (inCombat && Input.GetKeyDown("r")) {
				if (playerTarget != null) {
					updateCurrentActor(CombatManager.processTurn(combatants, inCombat, playerTarget));					
				}
				else {
					updateCurrentActor(CombatManager.processTurn(combatants, inCombat));
				}
			}
			if (inCombat && Input.GetKeyDown("f")) {
				if (gameObject.TryGetComponent(out CombatActor playerActor)) {
					playerTarget = playerActor.target(targetCounter);
					targetIndicator.transform.position = (playerTarget.transform.position + (2*Vector3.up));
				}
			}
			if (Input.GetKeyDown("k")) {
				if (tileList.Count - maxMoves >= 0) {transform.Translate(tempVector);}
				tempVector = Vector3.zero;
				wipeTiles();
				grid.transform.position = transform.position;
				grid.transform.Translate(new Vector3 (0,-0.3f, 0));
				if (!inCombat) {
					replenishAP();
				}
			}

		}
		else // The logic of free, unrestricted movement. Includes jumping.
		{
			if(materiaali.GetColor("_Color") != Color.white) {
				materiaali.SetColor("_Color",Color.white);
			}
			
			Vector3 mvmnt = new Vector3(0, 0 , verticalInput);
			Vector3 rotate = new Vector3(0, horizontalInput*Time.deltaTime*120, 0);
			controller.Move(transform.TransformDirection(mvmnt) * moveSpeed * Time.deltaTime);
			transform.Rotate(rotate, Space.Self);
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
			audioManager.swapExplorationTrack();
			flipTurnbased();
		}			
		
    }
	// Universal function for flipping the turn-based mode
	private void flipTurnbased() {
		//tempVector = Vector3.zero;
		if (turnBased) { // Free-play mode is engaged
			turnBased = false;
			//grid.transform.position = transform.position;
			grid.transform.Translate(Vector3.zero);
			modeButtonText = "Free-roam";
			hideIndicator();
			wipeTiles();
		}
		else { // Turn-based mode is engaged, the amount of moves a player has
			grid.transform.position = transform.position;
			grid.transform.Translate(new Vector3 (0,-0.3f, 0));
			turnBased = true;
			modeButtonText = "Turn-based";
			gameObject.transform.rotation = Quaternion.identity;
			maxMoves = 5;
			//resetTurnBased();
			Debug.Log("Moves remaining: " + maxMoves);
		}
	}
	
	public void replenishAP() {
		maxMoves = 5;
	}
	
	public void resetTurnBased() {
		maxMoves = 5;
		//tileList.Clear();
		tempVector = Vector3.zero;
	}
	
	public void wipeTiles() {
		foreach (GameObject obj in tileList) {
			Destroy(obj);
		}
		tileList.Clear();
		tempVector = Vector3.zero;
	}
	
	public List<CombatActor> combatants = new List<CombatActor>();
	// Called when combat starts
	public void initCombat() {
		if(!turnBased) {flipTurnbased();}
		combatants = new List<CombatActor>();
		CombatManager.findCombatants(combatants);
		if (!inCombat) {audioManager.swapCombatTrack();} //Prevents audio restart when 'restarting' combat on new enemy entry
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
	
	// Places tile at given vector co-ordinates
	// Intended for use in turn-based mode with the grid
	// Requires baseTile and baseGrid to be within the scene to function
	private List<GameObject> tileList = new List<GameObject>();
	void placeTile(Vector3 moveDirection) { //moveDirection is directional vector, ex.(1,0,0)
		//Instantiate from playerpos if first tile, otherwise from previous tile
		GameObject origTile = GameObject.Find("baseTile");
		Vector3 tilePos = moveDirection;
		if (tileList.Count == 0) {
			tilePos += transform.position;
		} else {
			tilePos += tileList[tileList.Count - 1].transform.position;
		}
		tilePos.y = transform.position.y - 0.3f;
		if (tileList.Count > 1 && tileList[tileList.Count-2].transform.position == tilePos) {
			tempVector -= (moveDirection + (tileList[tileList.Count-1].transform.position - tileList[tileList.Count-2].transform.position));
			//tempVector -= (moveDirection + (tileList[tileList.Count-1].transform.position - transform.position));
			Destroy(tileList[tileList.Count-1]);
			tileList.RemoveAt(tileList.Count-1);
			maxMoves += 2;
			return;
		}
		else if (tileList.Count == 1 && (transform.position.x == tilePos.x && transform.position.z == tilePos.z )) {
			tempVector = Vector3.zero;
			Destroy(tileList[tileList.Count-1]);
			tileList.RemoveAt(tileList.Count-1);
			maxMoves += 2;
			return;
		}
		foreach(GameObject tile in tileList) {
			if (tile.transform.position == tilePos) {
				maxMoves++; //Spent point is refunded
				tempVector -= moveDirection;
				return;
			}
		}
		tileList.Add(Instantiate(origTile, tilePos, Quaternion.identity));
		
		/*GameObject newTile = new GameObject("tile"+(tileList.Count + 1));
		newTile.AddComponent<MeshFilter>();
		newTile.GetComponent<MeshFilter>().mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Meshes/tile.fbx", typeof(Mesh));
		newTile.AddComponent<MeshRenderer>();*/
	}		
	
	// Default text of the button
	public string modeButtonText = "Free-roam";
	private string targetText = "No target";
	private string actorText = "Nobody";
	private string latestAction = "Nothing happened";
	// GUI-elements for HUD, current implementation contains a button for movement mode which reactively changes its' text
	void OnGUI() {
		GUI.Button(new Rect(0, Screen.height-300, 150, 50), ("Targeting: "+targetText));
		GUI.Button(new Rect(Screen.width-300, Screen.height-300, 250, 50), ("Current turn: "+actorText));
		GUI.Button(new Rect(Screen.width-350, Screen.height-250, 350, 50), ("Combat log \n "+latestAction));
		GUI.Button(new Rect(0, Screen.height-250, 200, 130), ("Interact with E. \n Cycle targets in combat with F. \n You have 5 AP per turn in combat \n Attack with Q. (2 AP) \n Activate Dodge with Z (1 AP) \n Move with tiles (1 AP/tile). \n Confirm movement with K. \n End turn with R"));
		if (GUI.Button (new Rect(0, Screen.height-110, 100, 50), modeButtonText) && !inCombat) {
			if (turnBased) {
			//turnBased = false;
				flipTurnbased();
				//modeButtonText = "Free-roam";
				//Debug.Log("HÄH");
			}
			else {
				//turnBased = true;
				flipTurnbased();
				//modeButtonText = "Turn-based";
			}
		}
    }
	
	public void updateTarget(string targetName) {
		targetText = targetName;
	}
	public void updateCurrentActor(string currentActor) {
		actorText = currentActor;
	}
	public void logAction(string actionDesc){
		latestAction = actionDesc;
	}
	public void hideIndicator() {
		targetIndicator.transform.position = new Vector3(0,-3,0);
		updateTarget("None");
	}
	
}
