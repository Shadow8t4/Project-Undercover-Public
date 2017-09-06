using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Simple script for a test I need to do

public class PlayerController : MonoBehaviour {

	public float jumpPower;
    public GameObject rocketPrefab;

	// Use this for initialization
	void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {

        //  Basic jumping
		if (Input.GetKeyDown (KeyCode.Space)) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, jumpPower);
		}

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rocketPos = transform.position;
            GameObject rocket = Instantiate(rocketPrefab, rocketPos, Quaternion.identity);
            rocket.GetComponent<RocketController>().player = this;
        }            	
	}
}
