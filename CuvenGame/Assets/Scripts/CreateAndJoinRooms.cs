using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    RoomOptions roomOptions = new RoomOptions();

    int maxPlayers = 8;

    void Start()
    {
        // Syncs all players when changing a scene
        //PhotonNetwork.AutomaticallySyncScene = true;

        // Input limnitations
        createInput.characterLimit = 10;
        joinInput.characterLimit = 10;

        roomOptions.MaxPlayers = System.Convert.ToByte(maxPlayers);
    }

    // Creating a room
    public void CreateRoom()
    {
        if (createInput.text.Length > 1)
        {
            PhotonNetwork.CreateRoom(createInput.text.ToUpper(), roomOptions);
        }
    }

    // Joinging a room
    public void JoinRoom()
    {

        if (joinInput.text.Length > 1)
        {
            PhotonNetwork.JoinRoom(joinInput.text.ToUpper());
        }
    }

    /*
       public virtual void OnCreateRoomFailed(short returnCode, string message)
       {
           Debug.Log("failed");
       }


       public virtual void OnJoinRoomFailed(short returnCode, string message)
       {
           Debug.Log("failed");
       }

   */

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Room");
    }
}
