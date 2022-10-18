using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;                                          // Room name text

    public RoomInfo RoomInfo { get; private set; }                          // Get and set room info

    public void SetRoomInfo(RoomInfo roomInfo)                              
    {
        RoomInfo = roomInfo;
        //_text.text = roomInfo.Name + ", " + roomInfo.MaxPlayers ;
        _text.text = roomInfo.Name;
    }

    public void OnClick_Button()                                            // Click on button to join the room and set the room's name to RoomInfo/Name
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
