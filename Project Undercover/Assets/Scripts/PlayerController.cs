using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
	// Testing
	public GameObject particle;
	// Player as a nav mesh agent
	private NavMeshAgent agent;

	void Start() 
	{
		agent = GetComponent<NavMeshAgent> (); 
	}

	void Update()
	{
		/* // WASD or Arrow keys movement
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
		*/

		RaycastHit hit;
		// Testing
		if (Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				Vector3 wantedPos = hit.point;
				wantedPos.y = 0.5f;
				Instantiate (particle, wantedPos, transform.rotation);
			}
		}
		// Walking
		else if (Input.GetButtonDown("Fire2")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				Vector3 wantedPos = hit.point;
				wantedPos.y = 0.5f;
				agent.destination = wantedPos;
			}
		}
	}
}