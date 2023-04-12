using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUsernameTest : MonoBehaviour
{
    Rigidbody rb;
    PhotonView view;

    public bool sceneChoiceDecided = false;
    public int sceneDecision;

    public TextMeshProUGUI username;
    string playersUsername;

    [SerializeField] float speed = 5f;
    [SerializeField] float speedModifier = 1f;
    [SerializeField] float jumpForce = 8f;

    public bool playerIsGrounded = false;

    public int playerNumber = -1;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();

        try
        {
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                {
                    playerNumber = i;
                }
            }
        }
        catch (NullReferenceException e) { }

    }

    void FixedUpdate()
    {
        // player name
        if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
        }
        this.name = playersUsername;

        if (view.IsMine)
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "rumble")
            {
                view.RPC("setUsername", RpcTarget.All, view.Owner.NickName);

                // USERNAME SCENE - players picking their name and character skin
                if (SceneManager.GetActiveScene().name == "Username 1")
                {
                    // begin game
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().beginCharSelection)
                        {
                            view.RPC("beginTheGame", RpcTarget.All);
                        }
                    }

                    MovePlayer();

                    if (Input.GetKeyDown(KeyCode.Space) && playerIsGrounded)
                    {
                        Vector3 vel = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                        view.RPC("jumpBoyJump", RpcTarget.All, view.Owner.NickName, vel);
                    }
                }
            }
        }
    }


    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 playerPos = rb.position;
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        Quaternion targetRotation;


        if (movement == Vector3.zero)
        {
            return;
        }
        else
        {
            targetRotation = Quaternion.LookRotation(movement);

            targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                360 * Time.fixedDeltaTime), 0.8f);

        }

        rb.MovePosition(playerPos + movement * speedModifier * speed * Time.fixedDeltaTime);
        rb.MoveRotation(targetRotation);

    }



    [PunRPC]
    void beginTheGame()
    {
        try
        {

            GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState = "characterSelection";
            Debug.Log("curent state is " + GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState);

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Username");
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void getPlayersNickName(string name)
    {
        try
        {
            playersUsername = name;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void setUsername(string Player)
    {
        try
        {
            GameObject.Find(Player).GetComponent<PlayerUsernameTest>().username.text = Player;

            GameObject.Find(Player).GetComponent<PlayerUsernameTest>().username.transform.LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).GetComponent<PlayerUsernameTest>().username.transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
