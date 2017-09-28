using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomStatusPanel : MonoBehaviour {

    public Text roomNameText;
    public Text numPlayersText;

    public void SetInformation(RoomInfo info)
    {
        roomNameText.text = info.Name;
        numPlayersText.text = info.PlayerCount.ToString() + "/" + info.MaxPlayers.ToString();
    }
}
