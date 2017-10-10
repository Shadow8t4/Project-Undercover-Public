using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleanUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Cleanup());
	}
	
    IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
