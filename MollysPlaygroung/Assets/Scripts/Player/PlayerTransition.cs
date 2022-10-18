using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;

public class PlayerTransition : MonoBehaviour
{
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Transition" || SceneManager.GetActiveScene().name == "Game End" || SceneManager.GetActiveScene().name == "Game Intro")
        {
            if (view.IsMine)
            {
                view.RPC("playerCleanUp", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
            }
        }
    }

    [PunRPC]
    void playerCleanUp(string Player)
    {
        try
        {
            GameObject.Find(Player).transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find(Player).GetComponent<SpriteRenderer>().color = new Color(GameObject.Find(Player).GetComponent<SpriteRenderer>().color.r, GameObject.Find(Player).GetComponent<SpriteRenderer>().color.g, GameObject.Find(Player).GetComponent<SpriteRenderer>().color.b, 0f);
        }
        catch (NullReferenceException e)
        {
            // error
        }

    }

}