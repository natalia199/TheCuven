using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

public class die_PlayerMovement : MonoBehaviour
{
    public TextMeshProUGUI username;
    string playersUsername = "die pls";

    private Rigidbody rigidBody;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 8f;
    die_PlayerCombat combatState;
    public bool isJumping = false;

    public float fallMultiplier = 5f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        combatState = GetComponent<die_PlayerCombat>();
    }

    void Update()
    {
        this.name = playersUsername;
        setUsername(this.name);
        
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
    void getPlayersNickName(string p)
    {
        playersUsername = p;
    }

    void setUsername(string Player)
    {
        try
        {
            GameObject.Find(Player).GetComponent<die_PlayerMovement>().username.text = Player;
            GameObject.Find(Player).GetComponent<die_PlayerMovement>().username.transform.LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).GetComponent<die_PlayerMovement>().username.transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

}
