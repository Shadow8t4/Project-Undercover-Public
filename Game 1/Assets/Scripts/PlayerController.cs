using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//  Simple script for a test I need to do

public class PlayerController : MonoBehaviour {

	public float jumpPower;
    public GameObject rocketPrefab;

	public Text winText;

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

	void OnTriggerEnter2D(Collider2D other)
	{
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag ("Finish Line")) {
			winText.text = "You Win!";
		}
		
		// Deathbox and saws restart level
        if (other.gameObject.CompareTag ("Deathbox")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}
