using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : Photon.PunBehaviour {

    public GuardCameraController guardCamera;
    public GameObject spyPrefab, NCPPrefab;
    private int numNCPs = 9;
    public int spyMissionsComplete = 0;
    public float waitBetweenMissions = 5.0f;
    public bool onMissionCooldown = false;
    public Text missionsCompleteText;
    public GameObject winPanel;
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

    void Awake()
    {
        if (PersistantPlayerSettings.character == PersistantPlayerSettings.Character.Guard)
        {
            guardCamera.SetCameraEnabled(guardCamera, true);
        }
        else
        {
            guardCamera.GetComponent<GuardCamera>().laserSightEnabled = true;
            Vector3 randPos = SimpleNPCBehavior.GetRandomLocation();
            var spy = PhotonNetwork.Instantiate(spyPrefab.name, randPos, Quaternion.identity, 0);
            spy.GetComponent<PlayerController>().enabled = true;
            spy.GetComponent<Spy>().SetColor();
        }
        /*
        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < numNCPs; i++)
            {
                Vector3 randPos = SimpleNPCBehavior.GetRandomLocation();
                PhotonNetwork.Instantiate(NCPPrefab.name, randPos, Quaternion.identity, 0);
            }
        }*/

    }

    public void CompleteMission()
    {
        if (!PhotonNetwork.isMasterClient || onMissionCooldown)
            return;
        StartCoroutine(MissionCooldown());
        spyMissionsComplete++;
        photonView.RPC("CompleteMissionRPC", PhotonTargets.All, spyMissionsComplete);

        if (spyMissionsComplete >= numOfMissions)
            photonView.RPC("ShowSpiesWinScreen", PhotonTargets.All);
    }

    [PunRPC]
    void CompleteMissionRPC(int missionsCompleted)
    {
        spyMissionsComplete = missionsCompleted;
        missionsCompleteText.text = spyMissionsComplete + "/3";
    }

    [PunRPC]
    void ShowSpiesWinScreen()
    {
        winPanel.SetActive(true);
        winText.text = "SPIES WIN!";
    }

    [PunRPC]
    void ShowGuardsWinScreen()
    {
        winPanel.SetActive(true);
        winText.text = "GUARDS WIN!";
    }

    IEnumerator MissionCooldown()
    {
        onMissionCooldown = true;
        yield return new WaitForSeconds(waitBetweenMissions);
        onMissionCooldown = false;
    }
}
