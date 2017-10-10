using UnityEngine;
using UnityEngine.UI;


public class Launcher : Photon.PunBehaviour
{
    public byte MaxPlayersPerRoom = 4;

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    public GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    public Text progressLabel;

    public GameObject nameSelectorPanel, roomSelectorPanel;

    public GameObject availableRoomsPanel;

    public GameObject roomStatusPanelPrefab;

    string _gameVersion = "1";
    bool isConnecting;

    void Awake()
    {
        // #Critical
        // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = false;

        // #Critical
        // This makes sure we can use PhotonNetwork.LoadLevel() on the master client
        // and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
    }

    void Start()
    {
        controlPanel.SetActive(true);
    }

    public void Connect()
    {
        isConnecting = true;
        progressLabel.text = "Connecting...";
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");
        if (isConnecting)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Launcher: Entered Lobby");
        nameSelectorPanel.SetActive(false);
        roomSelectorPanel.SetActive(true);
        RefreshRoomsList();
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        RefreshRoomsList();
    }

    public override void OnReceivedRoomListUpdate()
    {
        RefreshRoomsList();
    }

    public void RefreshRoomsList()
    {
        RoomInfo[] roomsList = PhotonNetwork.GetRoomList();
        foreach (Transform child in availableRoomsPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (RoomInfo roomInfo in roomsList)
        {
            var panel = Instantiate(roomStatusPanelPrefab, availableRoomsPanel.transform);
            var roomStatusPanel = panel.GetComponent<RoomStatusPanel>();
            roomStatusPanel.SetInformation(roomInfo);
            roomStatusPanel.launcher = this;
        }
    }

    public void RoomSelected(RoomStatusPanel panel)
    {
        PhotonNetwork.JoinRoom(panel.GetRoomName());
    }

    public void CreateNewRoom()
    {
        string roomName = PhotonNetwork.playerName + "'s Room";
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
        PhotonNetwork.LoadLevel("Lobby");
    }
}

