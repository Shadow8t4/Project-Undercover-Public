using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyCameraController : MonoBehaviour {

	private float camSens = 100.0f;
	private float xRotation;

	void Start () {
		xRotation = 0.0f;	
	}

	void LateUpdate () {
		xRotation = Time.deltaTime * Input.GetAxis ("Horizontal") * camSens;
		transform.RotateAround (Vector3.zero, Vector3.up, -xRotation);
	}
}
