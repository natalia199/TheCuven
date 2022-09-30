using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //Connecting to the photon server
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //Once we connected, we join the lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //Once we join the lobby, we load up our lobby scene
        SceneManager.LoadScene("Lobby");
    }
}
