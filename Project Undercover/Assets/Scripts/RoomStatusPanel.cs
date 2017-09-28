using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomStatusPanel : MonoBehaviour {

    public Text roomNameText;
    public Text numPlayersText;
    public Launcher launcher;
    private int numPlayers, maxPlayers;

    public void SetInformation(RoomInfo info)
    {
        roomNameText.text = info.Name;
        numPlayers = info.PlayerCount;
        maxPlayers = info.MaxPlayers;
        numPlayersText.text = numPlayers.ToString() + "/" + maxPlayers.ToString();
        if (numPlayers == maxPlayers)
        {
            numPlayersText.color = Color.red;
        }
    }

    public void RoomSelected()
    {
        if (numPlayers <= maxPlayers)
        {
            launcher.RoomSelected(this);
        }
    }

    public string GetRoomName()
    {
        return roomNameText.text;
    }
}
