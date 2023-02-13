using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PlayerBadgeActions : MonoBehaviourPunCallbacks
{
    Camera playerCam;

    // View (PhotonNetwork)
    PhotonView view;
                  // Tracks when username is SELECTED

    public bool waitingRoomSpawn = true;                    // Tracks when username is SELECTED

    string playerActorNumber;                       // Player's actor number (e.g. P1) 

    public TextMeshProUGUI username;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
        }
        this.name = playerActorNumber;

        if (view.IsMine)
        {
            view.RPC("setUsername", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
        }
    }

    public void DisconnectingPlayer()
    {
        Debug.Log("P" + PhotonNetwork.LocalPlayer.ActorNumber.ToString() + " disconnecting");
    }

    [PunRPC]
    void setUsername(string name, string Player)
    {
        try
        {
            GameObject.Find(Player).GetComponent<PlayerBadgeActions>().username.text = name;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // Sharing owner's player number with others
    [PunRPC]
    void getPlayerNumber(string name)
    {
        playerActorNumber = name;
    }

}
