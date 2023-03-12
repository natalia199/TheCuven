using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class UsernameMenu : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI usernameInputText;

    public TextMeshProUGUI warning;

    private void Awake()
    {
        usernameInputText.GetComponentInParent<TMP_InputField>().characterLimit = 10;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName.Length > 1)
            {
            }
            else
            {
                player.NickName = "oogabooga";
            }
        }
    }

    public void OnClick_EnterUsername()
    {
        if (usernameInputText.text.Length > 1)
        {
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if(PhotonNetwork.PlayerList[i].NickName == usernameInputText.text)
                {
                    StartCoroutine("DisplayInvalidWarning", 3f);
                    break;
                }

                if (i == PhotonNetwork.PlayerList.Length - 1)
                {
                    PhotonNetwork.LocalPlayer.NickName = usernameInputText.text;

                    Debug.Log("My nickname is " + PhotonNetwork.LocalPlayer.NickName);

                    GameObject.Find("Scene Manager").GetComponent<SceneManage>().switchCamera(false);

                    //PhotonNetwork.LoadLevel("PlayerRumble");
                }
            }
        }
        else
        {
            StartCoroutine("DisplayErrorWarning", 3f);
        }
    }

    /*
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Failed to conntect to Photon " + cause.ToString(), this);
    }
    */

    IEnumerator DisplayErrorWarning(int value)
    {
        warning.text = "Invalid username! Please try again.";

        yield return new WaitForSeconds(value);

        warning.text = "";
    }

    IEnumerator DisplayInvalidWarning(int value)
    {
        warning.text = "Username already taken! Please try another name.";

        yield return new WaitForSeconds(value);

        warning.text = "";
    }
}
