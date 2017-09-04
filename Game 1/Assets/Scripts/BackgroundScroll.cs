using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

    private Rigidbody2D lowerBod;

	// Use this for initialization
	void Start () {
        lowerBod = GetComponent<Rigidbody2D>(Background1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
