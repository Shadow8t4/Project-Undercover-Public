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

		minX = -2.0f;
		maxX = 2.0f;
		minY = 0.0f;
		maxY = 18.0f;
	}

	void LateUpdate () {
		newPosition = player.transform.position + offset;

		transform.position = new Vector3
			(
				Mathf.Clamp (newPosition.x, minX, maxX),
				//transform.position.x,
				Mathf.Clamp (newPosition.y, minY, maxY),
				transform.position.z
			);
		
	}
}