using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SimpleNPCBehavior : MonoBehaviour 
{
	public int updateTime;

	private Vector3 target;
	private NavMeshAgent agent;

	enum State {
		idle,
		walking,
		talking
	};

	void Start() 
	{
		this.GetComponent<Renderer> ().material.color = Random.ColorHSV (0f, 1f, 1f, 1f, 0f, 1f);
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