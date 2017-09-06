using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;
	private Vector3 newPosition;

	private float minX, maxX, minY, maxY;

	void Start () {
		offset = transform.position - player.transform.position;

		minX = 0.0f;
		maxX = 0.0f;
		minY = 0.0f;
		maxY = 18.0f;
	}

	void LateUpdate () {
		newPosition = player.transform.position + offset;

		if(newPosition.x >= minX && newPosition.x <= maxX && newPosition.y >= minY && newPosition.y <= maxY)
			transform.position = newPosition;
		
	}
}
