using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

public class main_PlayerMovement : MonoBehaviour
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

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            MovePlayer();

            // Jump
            if (Input.GetButtonDown("Jump") && isGrounded())
            {
                Vector3 vel = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
                view.RPC("Jumpers", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vel, true);
            }
            if (rigidBody.velocity.y < 0)
            {
                Vector3 vel = Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                view.RPC("Jumpers", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vel, false);

            }
        }
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        var isometricOffset = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        float moveSpeed = speed; //Making this a seperate variable to make speed adjustments for different animations

        Vector3 movement = isometricOffset.MultiplyPoint3x4(new Vector3(moveHorizontal, 0.0f, moveVertical));
        if (movement != Vector3.zero)
        {
            //flip looking direction if pulling to simulate walking backward
            if (combatState.isPulling || combatState.isDragged)
            {
                transform.rotation = Quaternion.LookRotation(-movement);
                moveSpeed = 3;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(movement);
            }
        }
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    float getVelocity(string axis)
    {
        switch (axis)
        {
            case "x":
                return rigidBody.velocity.x;
            case "y":
                return rigidBody.velocity.y;
            case "z":
                return rigidBody.velocity.z;
        }

        return 0.0f;
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
            GameObject.Find(Player).GetComponent<main_PlayerMovement>().username.text = Player;
            GameObject.Find(Player).GetComponent<main_PlayerMovement>().username.transform.LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).GetComponent<main_PlayerMovement>().username.transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // jumpies
    [PunRPC]
    void Jumpers(string player, Vector3 vel, bool pls)
    {
        try
        {
            if (pls)
            {
                GameObject.Find(player).GetComponent<Rigidbody>().velocity = vel;
                GameObject.Find(player).GetComponent<main_PlayerMovement>().isJumping = true;
            }
            else
            {
                GameObject.Find(player).GetComponent<Rigidbody>().velocity += vel;
                GameObject.Find(player).GetComponent<main_PlayerMovement>().isJumping = false;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
