using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : Photon.PunBehaviour {

    public GameObject guardController;
    public GameObject spyPrefab, NPCPrefab, cameraRigPrefab;
    public int numNpcs = 9;
    public int spyMissionsComplete = 0;
    public float waitBetweenMissions = 5.0f;
    public bool onMissionCooldown = false;
    public Text missionsCompleteText;
    public GameObject winPanel;
    public GameObject guardPanel;
    public GameObject spyPanel;
    public GameObject guardCameraPanel;
    public Text winText;
    private int numOfMissions = 3;

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
            guardController.SetActive(true);
            guardCameraPanel.SetActive(true);
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

    [PunRPC]
    void SpawnNPC(Vector3 pos)
    {
        Instantiate(NPCPrefab, pos, Quaternion.identity);
    }
}
