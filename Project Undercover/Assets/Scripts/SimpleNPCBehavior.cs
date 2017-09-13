using UnityEngine;
using System.Collections;

public class SimpleNPCBehavior : MonoBehaviour 
{
	public float moveSpeed;
	public int updateTime;
	private Rigidbody rigi;
	private Vector3 target;

	void Start() 
	{
		target.Set (4.0f - (8.0f * Random.value), 0.5f, 4.0f - (8.0f * Random.value));
		rigi = GetComponent<Rigidbody>();
	}

	void Update() 
	{    
		// Every updateTime seconds set new target position
		if (Time.fixedTime % updateTime == 0) {
			if ((int) (3.0f * Random.value) == 0)
				target.Set (4.0f - (8.0f * Random.value), 0.5f, 4.0f - (8.0f * Random.value));
		}

		// Check if NPC is at target and move there if not
		if ((target.x - transform.position.x >= 0.01) || (target.x - transform.position.x <= -0.01) || (target.y - transform.position.y >= 0.01) || (target.y - transform.position.y <= -0.01))
			transform.position += (target - transform.position).normalized * moveSpeed * Time.deltaTime;

		rigi.velocity = Vector3.zero;
	}
}