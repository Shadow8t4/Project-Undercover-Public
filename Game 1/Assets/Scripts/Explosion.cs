﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public PlayerController player;
    private float MAX_PLAYER_DIST = 3.0f;

	void Start () {
        Vector3 playerPos = player.transform.position;
        float dist = (playerPos - transform.position).magnitude;
        if (dist < MAX_PLAYER_DIST)
        {
            Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
            Vector2 force = (playerPos - transform.position).normalized * (MAX_PLAYER_DIST - dist) * 15.0f;
            playerBody.AddForce(force, ForceMode2D.Impulse);
            Debug.Log(force);
        }
	}
}
