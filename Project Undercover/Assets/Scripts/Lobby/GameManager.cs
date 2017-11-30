using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : Photon.PunBehaviour {

    public GameObject guardController;
    public GameObject spyPrefab, NPCPrefab, cameraRigPrefab;
    public GameObject missionPanel;
    public int numNpcs = 9;
    public State spyIdle;

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
            MissionTracker.IsGuard = true;
        }
        else
        {
            Vector3 randPos = StateController.GetRandomLocation();
            var spy = PhotonNetwork.Instantiate(spyPrefab.name, randPos, Quaternion.identity, 0);

            GameObject cameraRig = Instantiate(cameraRigPrefab, Vector3.zero, Quaternion.identity);
            cameraRig.GetComponentInChildren<ThirdPersonCameraController>().SetTarget(spy.transform);

            missionPanel.SetActive(true);
            MissionTracker.IsGuard = false;
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
        var newSpy = randNPC.gameObject;
        var controller = newSpy.GetComponent<StateController>();

        // Replace NPC with Spy
        PhotonView spyView = PhotonView.Find(spyId);
        if (spyView && spyView.isMine)
        {
            controller.photonView.TransferOwnership(spyView.owner);
            GameObject cameraRig = GameObject.FindGameObjectWithTag("CameraRig");
            cameraRig.GetComponentInChildren<ThirdPersonCameraController>().SetTarget(newSpy.transform);

            PhotonNetwork.Destroy(spyView.gameObject);
        }
        controller.name = "NewSpy";
        controller.tag = "Spy";
        controller.TransitionToState(spyIdle);
    }

    [PunRPC]
    void TransferNPCToPlayer(int npcId, int player)
    {

    }
}
