using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : Photon.PunBehaviour {

    public GameObject NPCPrefab;
	void Start () {
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.Instantiate(NPCPrefab.name, Vector3.zero, Quaternion.identity, 0);
	}
	
	void Update () {
		
	}
}
