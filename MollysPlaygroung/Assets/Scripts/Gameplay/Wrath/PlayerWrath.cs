using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class PlayerWrath : MonoBehaviour
{
    PhotonView view;

    bool oneAndDone;

    void Start()
    {
        view = GetComponent<PhotonView>();
        oneAndDone = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
    }

    /*
    void OnTriggerStay(Collider other)
    {
        try
        {
            if (view.IsMine)
            {
                if (other.name == "deathZone" && !oneAndDone && !GameObject.Find("GameManager").GetComponent<WrathGameManager>().gameover)
                {
                    view.RPC("FellOut", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
                    oneAndDone = true;
                }
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void FellOut(string player)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<WrathGameManager>().LoserList(player);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    */
}
