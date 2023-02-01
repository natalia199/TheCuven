using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

public class MoveTest : MonoBehaviour
{
    PhotonView view;

    public TextMeshProUGUI username;
    string playersUsername;

    private Rigidbody rigidBody;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 8f;
    main_PlayerCombat combatState;
    public bool isJumping = false;

    public float fallMultiplier = 5f;

    void Start()
    {
        view = GetComponent<PhotonView>();
        rigidBody = GetComponent<Rigidbody>();
        combatState = GetComponent<main_PlayerCombat>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
        }
        this.name = playersUsername;

        if (view.IsMine)
        {
            view.RPC("setUsername", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
        }
    }


    public bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }

    /// NETWORKING

    [PunRPC]
    void getPlayersNickName(string name)
    {
        playersUsername = name;
    }

    [PunRPC]
    void setUsername(string Player)
    {
        try
        {
            GameObject.Find(Player).GetComponent<PlayerEnvy_ZachyNati>().username.text = Player;
            GameObject.Find(Player).GetComponent<PlayerEnvy_ZachyNati>().username.transform.LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).GetComponent<PlayerEnvy_ZachyNati>().username.transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

}
