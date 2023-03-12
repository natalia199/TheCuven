using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Connecting player to the PN server
    void Start()
    {
        print("Connecting to server");
        PhotonNetwork.AutomaticallySyncScene = true;                                // Syncing all players views once they're in a room
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;         // Setting game version (not really needed)
        PhotonNetwork.ConnectUsingSettings();                                       // Connecting to the server!!!
    }

    // Successfully connected to server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server!");
        PhotonNetwork.LoadLevel("Start");                                
    }

    // Disconnected from the server
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Failed to conntect to Photon " + cause.ToString(), this);
    }

}
