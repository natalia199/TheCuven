using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI _roomName;

    public TextMeshProUGUI warning;

    private void Awake()
    {
        _roomName.GetComponentInParent<TMP_InputField>().characterLimit = 15;
    }

    public void OnClick_CreateRoom()
    {
        if (_roomName.text.Length > 1) 
        { 
            if (!PhotonNetwork.IsConnected)
                return;

            //CreateRoom
            //JoinedOrCreateRoom
            RoomOptions options = new RoomOptions();
            //options.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
        }
        else
        {
            StartCoroutine("DisplayErrorWarning", 3f);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully.", this);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message, this);
        StartCoroutine("DisplayErrorWarning", 3f);
    }

    IEnumerator DisplayErrorWarning(int value)
    {
        warning.text = "Invalid or unavailable room name! Please try again.";

        yield return new WaitForSeconds(value);

        warning.text = "";
    }
}
