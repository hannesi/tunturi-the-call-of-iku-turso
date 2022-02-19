using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class charControl : MonoBehaviour
{
	private CharacterController controller;
	private Vector3 pVelocity;
	private bool hasJumped = true;
	private bool turnBased = false;
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

    // Update is called once per frame
    void Update()
    {
		// The designated button changes the movement mode of the player by flipping the current mode
		if (Input.GetButtonDown("Fire3")) {
			flipTurnbased();
		}
        float verticalInput = Input.GetAxis("Vertical");
		float horizontalInput = Input.GetAxis("Horizontal");
		if (turnBased)  // Turn-based movement logic - Once the amount of moves has been used, controls are locked
		{
			if(materiaali.GetColor("_Color") != Color.green && materiaali.GetColor("_Color") != Color.red) {
				materiaali.SetColor("_Color",Color.green);
			}
			if (maxMoves > 0) {
				if (Input.GetKeyDown("d")) {
					transform.Translate(1,0,0);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
				}
				else if (Input.GetKeyDown("s")) {//TODO mainitse graafikoille transformin muutoksesta
					transform.Translate(0,1,0);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
				}
				else if (Input.GetKeyDown("a")) {
					transform.Translate(-1,0,0);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
				}
				else if (Input.GetKeyDown("w")) {
					transform.Translate(0,-1,0);
					maxMoves--;
					Debug.Log("Moves remaining: " + maxMoves);
				} // A visual indicator for 0 moves
				if (maxMoves == 0) { 
					materiaali.SetColor("_Color",Color.red);
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
	
	// Default text of the button
	public string modeButtonText = "Free-roam";
	// GUI-elements for HUD, current implementation contains a button for movement mode which reactively changes its' text
	void OnGUI() {
		if (GUI.Button (new Rect(0, Screen.height-150, 100, 50), modeButtonText)) {
			if (turnBased) {
			//turnBased = false;
				flipTurnbased();
				modeButtonText = "Free-roam";
				Debug.Log("HÄH");
			}
			else {
				//turnBased = true;
				flipTurnbased();
				modeButtonText = "Turn-based";
			}
		}
    }

}
