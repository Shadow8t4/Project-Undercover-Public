using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Simple script for a test I need to do

public class PlayerController : MonoBehaviour {

	public float jumpPower;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, jumpPower);
		}
		
	}
}
