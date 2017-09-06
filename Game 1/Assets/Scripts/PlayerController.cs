using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  Simple script for a test I need to do

public class PlayerController : MonoBehaviour {

	public float jumpPower;
    public AnimationClip explosion;
    private AnimationClip explode;

	public Text winText;

	// Use this for initialization
	void Start () {
        explode = (AnimationClip)Instantiate(explosion, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
		winText.text = "";
    }
	
	// Update is called once per frame
	void Update () {

        //  Basic jumping
		if (Input.GetKeyDown (KeyCode.Space)) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, jumpPower);
		}

        if (Input.GetMouseButtonDown(0))
        {
            //explode.Play();
            Debug.Log("Explosion at: " + Input.mousePosition);
        }
                	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag ("Finish Line")) {
			winText.text = "You Win!";
		}
	}
}
