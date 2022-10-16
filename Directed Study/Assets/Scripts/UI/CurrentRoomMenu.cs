using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CurrentRoomMenu : MonoBehaviour
{
    public TextMeshProUGUI _roomName;

    void Start()
    {
        _roomName.text = PhotonNetwork.CurrentRoom.Name;
    }
}
