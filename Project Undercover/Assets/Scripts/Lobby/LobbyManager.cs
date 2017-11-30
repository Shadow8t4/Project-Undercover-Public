using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : Photon.PunBehaviour {

    public GameObject playerPanelPrefab;
    public GameObject guardsPanel, spysPanel;
    private GameObject localPanel;
    public Button startButton;

    void Start()
    {
        CreateLocalPlayerPanel();
        if (!PhotonNetwork.isMasterClient)
            startButton.gameObject.SetActive(false);
    }

    void CreateLocalPlayerPanel()
    {
        if (localPanel != null)
            Debug.LogError("LobbyManager: localPlayerPanel already exists");
        CreatePlayerPanel(PhotonNetwork.playerName);
    }
    
    void CreatePlayerPanel(string playerName)
    {
        // if (!PhotonNetwork.isMasterClient)
        //    Debug.LogError("LobbyManager: Can't add player on non-master client");
        localPanel = PhotonNetwork.Instantiate(playerPanelPrefab.name, Vector3.zero, Quaternion.identity, 0);
        if (PhotonNetwork.room.PlayerCount % 2 == 1)
            ClickSwitchToGuards();
        else
            ClickSwitchToSpys();
        localPanel.GetComponent<PlayerLobbyPanelController>().SetPlayerName(playerName);
    }

    public void StartGame()
    {
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.NickName);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient);
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void ClickSwitchToGuards()
    {
        localPanel.transform.SetParent(guardsPanel.transform, false);
        PersistantPlayerSettings.character = PersistantPlayerSettings.Character.Guard;
    }

    public void ClickSwitchToSpys()
    {
        localPanel.transform.SetParent(spysPanel.transform, false);
        PersistantPlayerSettings.character = PersistantPlayerSettings.Character.Spy;
    }
}
