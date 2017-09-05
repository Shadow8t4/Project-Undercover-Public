using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).tagHash);
        //if (anim.GetCurrentAnimatorStateInfo(0).tagHash == 0)
        //    Destroy(gameObject);
	}
}
