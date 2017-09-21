using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyPanelController : Photon.MonoBehaviour {

    public Text playerName;

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }

    public string GetPlayerName()
    {
        return playerName.text;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(playerName.text);
            stream.SendNext(GetPanelName());
        }
        else
        {
            // Network player, receive data
            playerName.text = (string)stream.ReceiveNext();
            ChangePanel((string)stream.ReceiveNext());
        }
    }

    public string GetPanelName()
    {
        if (transform.parent == null)
            return "";
        return transform.parent.name;
    }

    public void ChangePanel(string panelName)
    {
        if (panelName == GetPanelName())
            return;
        else
            transform.SetParent(GameObject.Find(panelName).transform, false); 
    }
}
