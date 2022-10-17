using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class UsernameMenu : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI usernameInputText;

    public TextMeshProUGUI warning;

    private void Awake()
    {
        usernameInputText.GetComponentInParent<TMP_InputField>().characterLimit = 10;
    }

    public void OnClick_EnterUsername()
    {
        if (usernameInputText.text.Length > 1)
        {
            PhotonNetwork.NickName = usernameInputText.text;

            Debug.Log("My nickname is " + PhotonNetwork.LocalPlayer.NickName);

            PhotonNetwork.JoinLobby();
        }
        else
        {
            StartCoroutine("DisplayErrorWarning", 3f);
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Failed to conntect to Photon " + cause.ToString(), this);
    }

    IEnumerator DisplayErrorWarning(int value)
    {
        warning.text = "Invalid username! Please try again.";

        yield return new WaitForSeconds(value);

        warning.text = "";
    }
}
