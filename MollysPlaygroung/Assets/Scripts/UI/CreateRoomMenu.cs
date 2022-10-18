using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI _roomName;                                                          // room name

    public TextMeshProUGUI warning;                                                             // warning message for incorrect input

    private void Awake()
    {
        _roomName.GetComponentInParent<TMP_InputField>().characterLimit = 15;                   // username letter limit
    }

    public void OnClick_CreateRoom()
    {
        // If input is not empty
        if (_roomName.text.Length > 1)
        {
            if (!PhotonNetwork.IsConnected)                                                     // Checking if connected
                return;

            RoomOptions options = new RoomOptions();                                            // Room conditions; ex. 8 max players
            options.MaxPlayers = 8;
            PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);        // Creating room or joining room if already existing, takes you to room
        }
        else
        {
            StartCoroutine("DisplayErrorWarning", 3f);
        }
    }

    // Room has been successfully created
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully.", this);
    }

    // Failing to join or create a room for X reason(s)
    // Nothing will happen
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
