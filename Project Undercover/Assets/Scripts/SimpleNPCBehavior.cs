using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SimpleNPCBehavior : Photon.PunBehaviour 
{
	private NavMeshAgent agent;
    private bool setTarget = true;

    enum State {
		idle,
		walking,
		talking
	};

	void Start() 
	{
		agent = GetComponent<NavMeshAgent>();
        if (PhotonNetwork.isMasterClient) {
            photonView.RPC("TeleportToTarget", PhotonTargets.All, GetRandomLocation());
            photonView.RPC("SetColorRPC", PhotonTargets.All, new Vector3(Random.value, Random.value, Random.value));
			Debug.Log ("setting color");
        }
    }

	void Update() 
	{    
        if (!PhotonNetwork.isMasterClient)
            return;

        if (setTarget)
        {
            setTarget = false;
            StartCoroutine(UpdateDestination());
        }
	}

    IEnumerator UpdateDestination()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 10.0f));
        Vector3 location = GetRandomLocation();

        photonView.RPC("SetTarget", PhotonTargets.All, location);
        setTarget = true;
    }

    public static Vector3 GetRandomLocation()
    {
        var randTarget = new Vector3(5.0f - (10.0f * Random.value), 1.0f, 5.0f - (10.0f * Random.value));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randTarget, out hit, 1.0f, NavMesh.AllAreas))
            return hit.position;
        else
            return GetRandomLocation();
    }

    [PunRPC]
    void SetTarget(Vector3 target)
    {
        agent.destination = target;
    }

    [PunRPC]
    void TeleportToTarget(Vector3 target)
    {
        agent.Warp(target);
        agent.destination = target;
    }

    [PunRPC]
    void SetColorRPC(Vector3 color)
    {
        transform.Find("Body").GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z);
    }
}