using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class PlayerEnvy : MonoBehaviour
{
    PhotonView view;

    //public TextMeshProUGUI horseDisplay;
    string playersUsername;

    bool atShootingPad;
    public string horseName;
    bool movethefknhorse;

    public GameObject infoz;

    Rigidbody rb;
    public float moveSpeed;
    Vector3 keyboardMovement;

    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        //horseDisplay = GameObject.Find("HorseNamies").GetComponent<TextMeshProUGUI>();
        atShootingPad = false;
        movethefknhorse = false;
    }

    void Update()
    {
        /*if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
        }
        this.name = playersUsername;*/
        
        if (view.IsMine)
        {
            //view.RPC("setUsername", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

            //keyboardMovement.x = Input.GetAxisRaw("Horizontal");
            //keyboardMovement.z = Input.GetAxisRaw("Vertical");

            //horseDisplay.text = "#" + GameObject.Find(horseName).transform.GetChild(0).GetComponent<HorseFinishLine>().horseID + " - " + horseName;
        }
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            //rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);

            if (Input.GetKey(KeyCode.Return) && atShootingPad)
            {
                movethefknhorse = true;
            }
            else
            {
                movethefknhorse = false;
            }

            if (movethefknhorse)
            {
                float step = 2f * Time.deltaTime; // calculate distance to move
                Vector3 diepls = Vector3.MoveTowards(GameObject.Find(horseName).transform.GetChild(0).position, GameObject.Find(horseName).transform.GetChild(0).GetComponent<HorseFinishLine>().finishLinePoint.position, step);
                view.RPC("RaceTheHorse", RpcTarget.AllBufferedViaServer, horseName, diepls);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (view.IsMine)
        {
            if (other.name == horseName)
            {
                atShootingPad = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (view.IsMine)
        {
            if (other.name == horseName)
            {
                atShootingPad = false;
            }
        }
    }

    public void AssigningHorses()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("u is master");
            view.RPC("neighMofo", RpcTarget.AllBufferedViaServer);
        }
        else
        {
            Debug.Log("nah cuh");
        }
    }
    
    // Sharing owner's player number with others
   /* [PunRPC]
    void getPlayersNickName(string name)
    {
        playersUsername = name;
    }

    [PunRPC]
    void setUsername(string Player)
    {
        try
        {
            //GameObject.Find(Player).GetComponent<PlayerEnvy>().username.text = Player;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
   */
    [PunRPC]
    void RaceTheHorse(string horse, Vector3 pos)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameManager>().MoveHorse(horse, pos);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void neighMofo()
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameManager>().SetPlayerHorses();
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
