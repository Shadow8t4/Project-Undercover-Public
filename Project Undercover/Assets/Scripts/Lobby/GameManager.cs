﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : Photon.PunBehaviour {

    public GameObject guardController;
    public GameObject spyPrefab, NPCPrefab, cameraRigPrefab;
    public int numNpcs = 9;
    private static GameManager _activeManager = null;

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Launcher");
    }


    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects
        LeaveRoom();
    }

    public void LeaveRoom()
	{
        PhotonNetwork.LeaveRoom();
    }

    public static GameManager ActiveManager
    {
        get
        {
            return _activeManager;
        }
    }

    void Start()
    {
        if (_activeManager == null)
            _activeManager = this;
        else
            Debug.LogError("More than one game manager in the scene!");

        if (PersistantPlayerSettings.character == PersistantPlayerSettings.Character.Guard)
        {
            guardController.SetActive(true);
            //guardPanel.SetActive(true);
        }
        else
        {
            Vector3 randPos = StateController.GetRandomLocation();
            var spy = PhotonNetwork.Instantiate(spyPrefab.name, randPos, Quaternion.identity, 0);

            GameObject cameraRig = Instantiate(cameraRigPrefab, Vector3.zero, Quaternion.identity);
            cameraRig.GetComponentInChildren<ThirdPersonCameraController>().SetTarget(spy.transform);
            //spyPanel.SetActive(true);
        }
        
        if (PhotonNetwork.isMasterClient)
        {
			for (int i = 0; i < numNpcs; i++)
            {
                Vector3 randPos = StateController.GetRandomLocation();
				PhotonNetwork.Instantiate(NPCPrefab.name, randPos, Quaternion.identity, 0);
            }
        }
    }

    public List<StateController> GetNpcs()
    {
        var npcList = new List<StateController>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("NPC"))
        {
            npcList.Add(obj.GetComponent<StateController>());
        }
        return npcList;
    }

    [PunRPC]
    void ReplaceNPCWithSpyRPC(int spyId, int npcId)
    {
        var randNPC = PhotonView.Find(npcId);

        // Replace NPC with Spy
        PhotonView spyView = PhotonView.Find(spyId);
        if (spyView && spyView.isMine)
        {
            PhotonNetwork.Destroy(spyView.gameObject);
            var spy = PhotonNetwork.Instantiate(spyPrefab.name, randNPC.transform.position, randNPC.transform.rotation, 0);
            GameObject cameraRig = GameObject.FindGameObjectWithTag("CameraRig");
            cameraRig.GetComponentInChildren<ThirdPersonCameraController>().SetTarget(spy.transform);
        }
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(randNPC.gameObject);
        }
    }
}
