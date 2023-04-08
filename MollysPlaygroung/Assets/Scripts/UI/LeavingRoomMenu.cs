using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LeavingRoomMenu : MonoBehaviourPunCallbacks
{
    public void OnClick_LeaveRoom()
    {
        //PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.LeaveRoom(true);
        //PhotonNetwork.Disconnect();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Loading");
    /*
        PhotonNetwork.LoadLevel("Lobby");*/
    }
}
