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
		// Vaihdetaan ohjausmoodia, jos painetaan nappia
		if (Input.GetButtonDown("Fire1")) {
			if (turnBased) { // Asetetaan vapaaliikemoodi
				turnBased = false;
				materiaali.SetColor("_Color",Color.white);
			}
			else { // Asetetaan vuoropohjamoodi aktiiviseksi, vihreä väri merkkaa, että liikkeitä on jäljellä.
				turnBased = true;
				maxMoves = 5;
				materiaali.SetColor("_Color",Color.green);
			}
		}
        float verticalInput = Input.GetAxis("Vertical");
		float horizontalInput = Input.GetAxis("Horizontal");
		if (turnBased)  // Vuoropohjaisen liikkeen logiikka - Jokainen liike kuluttaa 'pisteen'
		{
			if (maxMoves > 0) {
				if (Input.GetKeyDown("d")) {
					transform.Translate(1,0,0);
					maxMoves--;
				}
				else if (Input.GetKeyDown("s")) {
					transform.Translate(0,0,-1);
					maxMoves--;
				}
				else if (Input.GetKeyDown("a")) {
					transform.Translate(-1,0,0);
					maxMoves--;
				}
				else if (Input.GetKeyDown("w")) {
					transform.Translate(0,0,1);
					maxMoves--;
				} // Jos liikkeet loppuvat, maalataan pelaaja punaiseksi
				if (maxMoves == 0) { 
					materiaali.SetColor("_Color",Color.red);
				}
			}

			/*switch (horizontalInput < 0)
			{	
				case false:
					transform.Translate(1,0,0);
					horizontalInput = 0.0f;
					break;
				case true:
					transform.Translate(-1,0,0);
					horizontalInput = 0.0f;
					break;
				default:
					switch (verticalInput < 0)
					{
					case false:
						transform.Translate(0,0,1);
						verticalInput = 0.0f;
						break;
					case true:
						transform.Translate(0,0,-1);
						verticalInput = 0.0f;
						break;
					default:
						break;
					}
					break;
			}*/
		}
		else // Vapaaliikkeen logiikka
		{
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


}
