using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : Photon.PunBehaviour {

    public GuardCameraController guardCamera;
    public GameObject spyPrefab, NPCPrefab, cameraRigPrefab;
    public int numNpcs = 9;


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

    void Start()
    {
        if (PersistantPlayerSettings.character == PersistantPlayerSettings.Character.Guard)
        {
            guardCamera.SetCameraEnabled(guardCamera, true);
            //guardPanel.SetActive(true);
        }
        else
        {
            guardCamera.GetComponent<GuardCamera>().spotLight.enabled = true;
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

    [PunRPC]
    void SpawnNPC(Vector3 pos)
    {
        Instantiate(NPCPrefab, pos, Quaternion.identity);
    }
}
