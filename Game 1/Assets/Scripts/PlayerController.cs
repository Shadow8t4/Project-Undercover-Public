using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Simple script for a test I need to do

public class PlayerController : MonoBehaviour {

	public float jumpPower;
    public AnimationClip explosion;
    private AnimationClip explode;

	// Use this for initialization
	void Start () {
        // explode = (AnimationClip)Instantiate(explosion, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {

        //  Basic jumping
		if (Input.GetKeyDown (KeyCode.Space)) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, jumpPower);
		}

        if (Input.GetMouseButtonDown(0))
        {

        }
                	
	}
}
