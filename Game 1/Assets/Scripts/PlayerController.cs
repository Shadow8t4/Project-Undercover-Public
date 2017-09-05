using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//  Simple script for a test I need to do

public class PlayerController : MonoBehaviour {

	public float jumpPower;
    public AnimationClip explosion;
    private AnimationClip explode;

	// Use this for initialization
	void Start () {
        explode = (AnimationClip)Instantiate(explosion, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
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

    // Handle collisions with Deathbox to trigger level restart.
    void OnTriggerEnter2D(Collider2D other) {

        // Deathbox and saws restart level
        if (other.gameObject.name == "Deathbox" || other.gameObject.name == "Saw") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
