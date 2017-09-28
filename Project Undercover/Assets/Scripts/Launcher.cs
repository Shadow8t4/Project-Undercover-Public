using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
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
        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
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
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
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
            // we don't want to do anything if we are not attempting to join a room. 
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()
                PhotonNetwork.JoinLobby();
                //PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Launcher: Entered Lobby");
            nameSelectorPanel.SetActive(false);
            roomSelectorPanel.SetActive(true);
            RefreshRoomsList();
        }

        public void RefreshRoomsList()
        {
            RoomInfo[] roomsList = PhotonNetwork.GetRoomList();
            Debug.Log("Number of rooms available: " + roomsList.Length);
            foreach (Transform child in availableRoomsPanel.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (RoomInfo roomInfo in roomsList)
            {
                var roomStatusPanel = Instantiate(roomStatusPanelPrefab, availableRoomsPanel.transform);
                roomStatusPanel.GetComponent<RoomStatusPanel>().SetInformation(roomInfo);
            }
        }

        public void CreateNewRoom()
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("Launcher: OnDisconnectedFromPhoton() was called by PUN");
        }

        /*
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.");
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        }*/

        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
}
