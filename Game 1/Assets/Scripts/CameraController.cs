using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private int offset;
	private Vector3 newPosition;

	private int minX, maxX, minY, maxY;

	void Start () {
		offset = transform.position.y - player.transform.position.y;
		minX = 0;
		maxX = 0;
		minY = 0;
		maxY = 18;
	}

	void LateUpdate () {
		newPosition = player.transform.position + offset;

		//if(newPosition.x >= minX && newPosition.x <= maxX && newPosition.y >= minY && newPosition.y <= maxY)
		if(newPosition.x >= minX && newPosition.x <= maxX && newPosition.y >= minY && newPosition.y <= maxY)
			
			transform.position = newPosition;
		
	}
}
