using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LeavingRoomMenu : MonoBehaviour
{
    private RoomsCanvases _roomsCanvases;

    public TextMeshProUGUI inputText;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        _roomsCanvases.CurrentRoomCanvas.Hide();
        inputText.text = "";
    }
}
