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

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "deathZone" && !oneAndDone)
        {
            Debug.Log("ur out");
            view.RPC("FellOut", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
            oneAndDone = true;
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
}
