using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResultScript : MonoBehaviour
{
    PhotonView view;

    public TextMeshProUGUI username;
    string playersUsername;

    public bool wheelSpun = false;

    public int playerNumber = -1;

    void Start()
    {
        view = GetComponent<PhotonView>();

        for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
            {
                playerNumber = i;
            }
        }

    }

    void FixedUpdate()
    {

        if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
        }
        this.name = playersUsername;
        this.transform.parent.name = playersUsername + "_Rect";

        if (view.IsMine)
        {
            view.RPC("setUsername", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0] == PhotonNetwork.LocalPlayer.NickName && !wheelSpun)
            {
                GameObject.Find("GameManager").GetComponent<ResultGameManager>().WheelOfFortune.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().WheelOfFortune.transform.GetChild(0).GetComponent<WheelSpin>().increasing = false;
                    wheelSpun = true;

                    Debug.Log("DONE SPIN");
                }
            }

            if (!GameObject.Find("GameManager").GetComponent<ResultGameManager>().countdownIsRunning)
            {
                view.RPC("setPlayerPosition", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

                for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                {
                    try
                    {
                        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);

                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive)
                        {
                            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].originalMesh;
                        }
                        else
                        {
                            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().deadSkin;
                        }
                    }

                    catch (NullReferenceException e) { }
                }
            }
        }
    }

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
            GameObject.Find(Player).GetComponent<PlayerResultScript>().username.text = Player;
            GameObject.Find(Player).GetComponent<PlayerResultScript>().username.transform.LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).GetComponent<PlayerResultScript>().username.transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
   
    [PunRPC]
    void setPlayerPosition(string Player)
    {
        try
        {
            GameObject.Find(Player + "_Rect").transform.parent = GameObject.Find("GameManager").GetComponent<ResultGameManager>().gridGameObject.transform;
            GameObject.Find(Player + "_Rect").transform.position = new Vector3(GameObject.Find(Player + "_Rect").transform.position.x, GameObject.Find(Player + "_Rect").transform.position.y, GameObject.Find("GameManager").GetComponent<ResultGameManager>().gridGameObject.transform.position.z);
            GameObject.Find(Player + "_Rect").transform.localScale = GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerScale;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void settingCharacterSkins()
    {
        try
        {
            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
            {
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive)
                {
                    try
                    {
                        for (int y = 0; y < GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).childCount; y++)
                        {
                            if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).name == GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].chosenCharacter)
                            {
                                GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).gameObject.SetActive(true);
                            }
                            else
                            {
                                Destroy(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).gameObject);
                            }

                        }
                    }
                    catch (NullReferenceException e) { }

                }
                else
                {
                    try
                    {
                        for (int y = 0; y < GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).childCount; y++)
                        {
                            if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).name == GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].chosenCharacter)
                            {
                                GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).gameObject.SetActive(true);
                                GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().deadSkin;
                            }
                            else
                            {
                                Destroy(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).gameObject);
                            }

                        }
                    }
                    catch (NullReferenceException e) { }

                }
            }
        }
        catch (NullReferenceException e) { }
    }
}

