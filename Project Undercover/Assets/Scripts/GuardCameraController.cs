using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCameraController : MonoBehaviour {

	private float camSens = 120.0f;
	private float xRotation = 0.0f;
	private float yRotation = 0.0f;

	void Start () {
		xRotation = transform.eulerAngles.y;
		yRotation = transform.eulerAngles.x;
	}
	
	void Update () {
		xRotation += Time.deltaTime * Input.GetAxis ("Horizontal") * camSens;
		yRotation += Time.deltaTime * Input.GetAxis ("Vertical") * camSens;
		transform.localRotation = Quaternion.AngleAxis (xRotation, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis (yRotation, -Vector3.right);
	}
}