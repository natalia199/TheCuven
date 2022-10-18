using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStart : MonoBehaviour
{
    PhotonView view;

    string playersUsername;


    void Start()
    {
        view = GetComponent<PhotonView>();

        // Naming player's gameobject corresponding actor number
        if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }

        this.name = playersUsername;

        if (view.IsMine)
        {
            StartCoroutine("PlayerNameUpdate", 2f);
        }
    }

    // Sharing owner's player number with others
    [PunRPC]
    void getPlayersNickName(string name)
    {
        playersUsername = name;
    }

    [PunRPC]
    void dontDestroyPlayer(string Player)
    {
        Debug.Log("player " + Player);
        DontDestroyOnLoad(GameObject.Find(Player));
    }

    IEnumerator PlayerNameUpdate(int value)
    {
        yield return new WaitForSeconds(value);

        view.RPC("dontDestroyPlayer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
    }
}
