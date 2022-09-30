using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RoomPlayerAction : MonoBehaviourPunCallbacks
{
    PhotonView view;

    public TextMeshProUGUI playerAmount;

    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            GameObject.Find("RoomName").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.Name;
            playerAmount = GameObject.Find("PlayerAmount").GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            playerAmount.text = "P: " + PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }
}