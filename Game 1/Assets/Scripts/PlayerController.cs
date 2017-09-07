using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//  Simple script for a test I need to do

public class PlayerController : MonoBehaviour {

	public float jumpPower;
    public GameObject rocketPrefab;
    private float reloadTime = 0.25f;
    private bool reloading = false;
	
	// Update is called once per frame
	void Update () {

        /*//  Basic jumping
		if (Input.GetKeyDown (KeyCode.Space)) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, jumpPower);
		}*/

        if (Input.GetMouseButtonDown(0) && !reloading)
        {
            reloading = true;
            StartCoroutine("Reload");
            Vector2 rocketPos = transform.position;
            GameObject rocket = Instantiate(rocketPrefab, rocketPos, Quaternion.identity);
            rocket.GetComponent<RocketController>().player = this;
            Physics2D.IgnoreCollision(rocket.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }            	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag ("Finish Line")) {
            LevelController.GetController().WinLevel();
        }
		
		// Deathbox and saws restart level
        if (other.gameObject.CompareTag ("Deathbox")) {
            LevelController.GetController().ResetLevel();
        }
	}

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        yield return null;
    }
}
