using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public string votingCardID;
    public string votedFor;
    public string votedPlayerName;

    /* public float maxScale;
     public float speed;

     public Vector3 v3OrgPos;
     public float orgScale;
     public float endScale;
    */

    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        atShootingPad = false;
        movethefknhorse = false;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            if (!GameObject.Find("GameManager").GetComponent<EnvyGameManager>().gameover)
            {
                if (Input.GetKey(KeyCode.Return) && atShootingPad && !GetComponent<PlayerEnvy_ZachyNati>().stunTheBitch)
                {
                    movethefknhorse = true;
                    float step = 10f * Time.deltaTime; // calculate distance to move
                    Vector3 diepls = Vector3.MoveTowards(GameObject.Find(horseName).transform.GetChild(0).position, GameObject.Find(horseName).transform.GetChild(0).GetComponent<HorseFinishLine>().finishLinePoint.position, step);

                    //Vector3 grr = new Vector3(GameObject.Find(horseName).transform.GetChild(1).localScale.x, GameObject.Find(horseName).transform.GetChild(1).localScale.y, Mathf.MoveTowards(transform.localScale.z, endScale, Time.deltaTime * speed));
                    //Vector3 pls = v3OrgPos + (GameObject.Find(horseName).transform.GetChild(1).forward) * (GameObject.Find(horseName).transform.GetChild(1).localScale.z / 2.0f + orgScale / 2.0f);
                    //endScale = maxScale;
                    view.RPC("RaceTheHorse", RpcTarget.AllBufferedViaServer, horseName, diepls);
                    //view.RPC("RaceTheHorse", RpcTarget.AllBufferedViaServer, horseName, diepls, grr, pls);
                }
                /*else if(!GameObject.Find("GameManager").GetComponent<EnvyGameManager>().votingSystem)
                {
                    movethefknhorse = false;
                    try
                    {
                        Vector3 grr = new Vector3(GameObject.Find(horseName).transform.GetChild(1).localScale.x, GameObject.Find(horseName).transform.GetChild(1).localScale.y, Mathf.MoveTowards(transform.localScale.z, endScale, Time.deltaTime * speed));
                        Vector3 pls = v3OrgPos + (GameObject.Find(horseName).transform.GetChild(1).forward) * (GameObject.Find(horseName).transform.GetChild(1).localScale.z / 2.0f + orgScale / 2.0f);
                        endScale = orgScale;
                        view.RPC("ReverseDaSquirt", RpcTarget.AllBufferedViaServer, horseName, Vector3.zero, grr, pls);
                    }
                    catch (NullReferenceException e)
                    {
                        // error
                    }
                }*/
                /*
                if (movethefknhorse)
                {
                    float step = 10f * Time.deltaTime; // calculate distance to move
                    Vector3 diepls = Vector3.MoveTowards(GameObject.Find(horseName).transform.GetChild(0).position, GameObject.Find(horseName).transform.GetChild(0).GetComponent<HorseFinishLine>().finishLinePoint.position, step);
                    view.RPC("RaceTheHorse", RpcTarget.AllBufferedViaServer, horseName, diepls);

                    GameObject.Find(horseName).transform.GetChild(1).position = new Vector3(GameObject.Find(horseName).transform.GetChild(1).localScale.x, GameObject.Find(horseName).transform.GetChild(1).localScale.y, Mathf.MoveTowards(GameObject.Find(horseName).transform.GetChild(1).localScale.z, endScale, Time.deltaTime * speed));
                    GameObject.Find(horseName).transform.GetChild(1).position = v3OrgPos + (GameObject.Find(horseName).transform.GetChild(1).forward) * (GameObject.Find(horseName).transform.GetChild(1).localScale.z / 2.0f + orgScale / 2.0f);
                    endScale = maxScale;
                }
                else
                {
                    GameObject.Find(horseName).transform.GetChild(1).position = new Vector3(GameObject.Find(horseName).transform.GetChild(1).localScale.x, GameObject.Find(horseName).transform.GetChild(1).localScale.y, Mathf.MoveTowards(GameObject.Find(horseName).transform.GetChild(1).localScale.z, endScale, Time.deltaTime * speed));
                    GameObject.Find(horseName).transform.GetChild(1).position = v3OrgPos + (GameObject.Find(horseName).transform.GetChild(1).forward) * (GameObject.Find(horseName).transform.GetChild(1).localScale.z / 2.0f + orgScale / 2.0f);
                    endScale = orgScale;
                }*/

                if (Input.GetMouseButtonDown(0) && GameObject.Find("GameManager").GetComponent<EnvyGameManager>().votingSystem)
                {
                    RaycastHit die;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out die, 100.0f))
                    {
                        if (die.transform.tag == "EnvyVote")
                        {
                            GameObject.Find("VotingChoice").GetComponent<TextMeshProUGUI>().text = "you picked " + GameObject.Find("GameManager").GetComponent<EnvyGameManager>().SendingVoterName(die.transform.name);
                            view.RPC("PlayerVote", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName, die.transform.name, GameObject.Find("GameManager").GetComponent<EnvyGameManager>().SendingVoterName(die.transform.name));
                        }
                    }
                }
                else if (!GameObject.Find("GameManager").GetComponent<EnvyGameManager>().votingSystem)
                {

                }
            }
        }
    }

    [PunRPC]
    void PlayerVote(string player, string vote, string votedPlayerName)
    {
        try
        {
            GameObject.Find(player).GetComponent<PlayerEnvy>().votedFor = vote;
            GameObject.Find(player).GetComponent<PlayerEnvy>().votedPlayerName = votedPlayerName;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == horseName)
            {
                atShootingPad = true;
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
            view.RPC("neighMofo", RpcTarget.AllBufferedViaServer);
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
            //GameObject.Find(player).GetComponent<PlayerEnvy>().endScale = GameObject.Find(player).GetComponent<PlayerEnvy>().orgScale;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    /*
    [PunRPC]
    void ReverseDaSquirt(string horse, Vector3 pos, Vector3 squirtPos, Vector3 scalaz)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameManager>().MoveHorse(horse, pos, squirtPos, scalaz, false);

            Debug.Log("c ya");
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }*/

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
