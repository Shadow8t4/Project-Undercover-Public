using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public GameObject particle;
	public float TransitionSpeed;
	private Rigidbody rigi;
	private Vector3 velocity = Vector3.zero;

	void Start() 
	{
		rigi = GetComponent<Rigidbody>();
	}

	void Update()
	{
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		RaycastHit hit;
		if (Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				Vector3 wantedPos = hit.point;
				wantedPos.y = 0.5f;
				Instantiate (particle, wantedPos, transform.rotation);
			}
		}
		if (Input.GetButtonDown("Fire2")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				Vector3 wantedPos = hit.point;
				wantedPos.y = 0.5f;
				transform.position = Vector3.Lerp (transform.position, wantedPos, Time.deltaTime * TransitionSpeed);
			}
		}
	}
}