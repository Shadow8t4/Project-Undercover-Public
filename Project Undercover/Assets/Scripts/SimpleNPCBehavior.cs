using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SimpleNPCBehavior : MonoBehaviour 
{
	public float moveSpeed;
	public int updateTime;
	private Vector3 target;
	private NavMeshAgent agent;

	void Start() 
	{
		target.Set (4.0f - (8.0f * Random.value), 0.5f, 4.0f - (8.0f * Random.value));
		agent = GetComponent<NavMeshAgent> (); 
	}

	void Update() 
	{    
		// Every updateTime seconds set new target position
		if (Time.fixedTime % updateTime == 0) {
			if ((int) (3.0f * Random.value) == 0)
				target.Set (4.0f - (8.0f * Random.value), 0.5f, 4.0f - (8.0f * Random.value));
		}
			
		agent.destination = target;
	}
}