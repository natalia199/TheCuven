using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class PlayerEnvy_ZachyNati : MonoBehaviour
{
    //[SerializeField] GluttonyLevel LevelController;
    PhotonView view;
    public TextMeshProUGUI username;
    string playersUsername;

    private Rigidbody rigidBody;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 8f;
    PlayerEnvy_Combat combatState;
    public bool isJumping = false;

    public float fallMultiplier = 5f;

    public bool stunTheBitch = false;

    void Start()
    {
        view = GetComponent<PhotonView>();

        rigidBody = GetComponent<Rigidbody>();
        combatState = GetComponent<PlayerEnvy_Combat>();
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

            MovePlayer();
            Jump();
        }
    }

    // Sharing owner's player number with others
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

    [PunRPC]
    void Jumpy(string player, Vector3 vel, bool pls)
    {
        try
        {
            if (pls) {
                GameObject.Find(player).GetComponent<Rigidbody>().velocity = vel;
                GameObject.Find(player).GetComponent<PlayerEnvy_ZachyNati>().isJumping = true;
            }
            else
            {
                GameObject.Find(player).GetComponent<Rigidbody>().velocity += vel;
                GameObject.Find(player).GetComponent<PlayerEnvy_ZachyNati>().isJumping = false;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded() && !stunTheBitch && !GameObject.Find("GameManager").GetComponent<EnvyGameManager>().votingSystem)
        {
            Vector3 velo = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
            //rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
            //isJumping = true;

            view.RPC("Jumpy", RpcTarget.AllBufferedViaServer, view.Owner.NickName, velo, true);
        }
        if (rigidBody.velocity.y < 0)
        {
            Vector3 velo = Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //fallMultipler - 1 accounts for build in gravity mutliplier
            //rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //fallMultipler - 1 accounts for build in gravity mutliplier
            //isJumping = false;
            view.RPC("Jumpy", RpcTarget.AllBufferedViaServer, view.Owner.NickName, velo, false);

        }
    }

    void MovePlayer()
    {
        if (!stunTheBitch && !GameObject.Find("GameManager").GetComponent<EnvyGameManager>().votingSystem)
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
    }

    public void CallStunah()
    {
        StartCoroutine("Stunah", 5);
    }

    IEnumerator Stunah(int value)
    {
        stunTheBitch = true;

        yield return new WaitForSeconds(value);

        stunTheBitch = false;
        GetComponent<PlayerEnvy_Combat>().theBitchIsStunned = false;
    }

    public bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }
}
