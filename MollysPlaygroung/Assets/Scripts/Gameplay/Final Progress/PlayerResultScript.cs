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

    public bool winnerVoting = false;
    public bool playerSabotageChoice = false;

    public bool deadPlayerChoice = false;
    public bool alivePlayerChoice = false;

    public string wheelResult;

    public int sabotagedPlayerChoice = -1;
    public int switcherooAlivePlayer;
    public int switcherooDeadPlayer;

    public bool winnersChoiceComplete = false;

    public bool oneTimeSend = false;

    Ray ray;

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

            if (!GameObject.Find("GameManager").GetComponent<ResultGameManager>().countdownIsRunning)
            {
                view.RPC("setPlayerPosition", RpcTarget.AllBufferedViaServer, view.Owner.NickName);


                // character skin
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


                // Wheel
                // WINNER
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0] == PhotonNetwork.LocalPlayer.NickName)
                {
                    if (GameObject.Find("GameManager").GetComponent<ResultGameManager>().SpinTheWheel)
                    {
                        GameObject.Find("GameManager").GetComponent<ResultGameManager>().WheelOfFortune.gameObject.SetActive(true);

                        if (GameObject.Find("GameManager").GetComponent<ResultGameManager>().accessToSpin)
                        {
                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = "Press Space to Stop";

                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                GameObject.Find("GameManager").GetComponent<ResultGameManager>().accessToSpin = false;
                                GameObject.Find("GameManager").GetComponent<ResultGameManager>().WheelOfFortune.transform.GetChild(0).GetComponent<WheelSpin>().increasing = false;
                                wheelSpun = true;
                                GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = "";
                                Debug.Log("DONE SPIN");
                            }
                        }

                        if (GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>().spinningHasFinished)
                        {
                            view.RPC("WheelResult", RpcTarget.AllBufferedViaServer, view.Owner.NickName, GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>().wheelDecision.GetComponent<WheelResult>().result);
                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().WheelOfFortune.gameObject.SetActive(false);
                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().SpinTheWheel = false;
                        }
                    }
                }

                // Picking players to sabotage
                if (winnerVoting)
                {
                    if (wheelResult == "Sabotage")
                    {
                        if (!playerSabotageChoice)
                        {
                            // character heads
                            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                            {
                                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username != GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0])
                                {
                                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).gameObject.SetActive(true);
                                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username;
                                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).GetChild(1).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);
                                }
                            }

                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0] == PhotonNetwork.LocalPlayer.NickName)
                            {
                                if (Input.GetMouseButtonDown(0))
                                {
                                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    RaycastHit[] die = Physics.RaycastAll(ray);
                                    foreach (RaycastHit hit in die)
                                    {
                                        if (hit.collider.gameObject.tag == "VoteHead")
                                        {
                                            sabotagedPlayerChoice = hit.collider.gameObject.GetComponent<VotedHeadInfo>().headID;
                                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = "Picked " + GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[sabotagedPlayerChoice].username;
                                            break;
                                        }
                                    }
                                }

                                if (sabotagedPlayerChoice != -1 && Input.GetKeyDown(KeyCode.Return))
                                {
                                    view.RPC("setVotedHead", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sabotagedPlayerChoice);
                                }
                            }
                        }
                        else
                        {
                            // character heads
                            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                            {
                                if (sabotagedPlayerChoice != x)
                                {
                                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                    else if (wheelResult == "Switcheroo")
                    {
                        if (!playerSabotageChoice)
                        {
                            if (!deadPlayerChoice)
                            {
                                // dead players pick first
                                for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                                {
                                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username != GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0])
                                    {
                                        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive)
                                        {
                                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).gameObject.SetActive(true);
                                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username;
                                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).GetChild(1).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);
                                        }
                                    }
                                }

                                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0] == PhotonNetwork.LocalPlayer.NickName)
                                {
                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                        RaycastHit[] die = Physics.RaycastAll(ray);
                                        foreach (RaycastHit hit in die)
                                        {
                                            if (hit.collider.gameObject.tag == "VoteHead")
                                            {
                                                switcherooDeadPlayer = hit.collider.gameObject.GetComponent<VotedHeadInfo>().headID;
                                                GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = "Picked " + GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[switcherooDeadPlayer].username;
                                                break;
                                            }
                                        }
                                    }

                                    if (switcherooDeadPlayer != -1 && Input.GetKeyDown(KeyCode.Return))
                                    {
                                        view.RPC("setSwitcharooDeadVote", RpcTarget.AllBufferedViaServer, view.Owner.NickName, switcherooDeadPlayer);
                                    }
                                }
                            }
                            else
                            {
                                // dead players pick first
                                for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                                {
                                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username != GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0])
                                    {
                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive)
                                        {
                                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).gameObject.SetActive(true);
                                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username;
                                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).GetChild(1).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);
                                        }
                                        else
                                        {
                                            GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).gameObject.SetActive(false);
                                        }
                                    }
                                }

                                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0] == PhotonNetwork.LocalPlayer.NickName)
                                {
                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                        RaycastHit[] die = Physics.RaycastAll(ray);
                                        foreach (RaycastHit hit in die)
                                        {
                                            if (hit.collider.gameObject.tag == "VoteHead")
                                            {
                                                switcherooAlivePlayer = hit.collider.gameObject.GetComponent<VotedHeadInfo>().headID;
                                                GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = "Picked " + GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[switcherooAlivePlayer].username;
                                                break;
                                            }
                                        }
                                    }

                                    if (switcherooAlivePlayer != -1 && Input.GetKeyDown(KeyCode.Return))
                                    {
                                        view.RPC("setSwitcharooAliveVote", RpcTarget.AllBufferedViaServer, view.Owner.NickName, switcherooAlivePlayer, switcherooDeadPlayer);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // character heads
                            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                            {
                                if (switcherooAlivePlayer != x || switcherooDeadPlayer != x)
                                {
                                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).gameObject.SetActive(false);
                                }
                                else if (switcherooAlivePlayer == x && switcherooDeadPlayer == x)
                                {
                                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().playerHeads.transform.GetChild(x).gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                }


                // LOSER
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0] != PhotonNetwork.LocalPlayer.NickName && GameObject.Find("GameManager").GetComponent<ResultGameManager>().SpinTheWheel && !wheelSpun)
                {
                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = "Winner deciding...";
                }
            }

            if (winnersChoiceComplete) 
            {
                GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsLoser = new List<string>();
                GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner = new List<string>();

                StartCoroutine("likklePause");
            }
        }
    }

    IEnumerator likklePause()
    {
        yield return new WaitForSeconds(3f);

        if (PhotonNetwork.LocalPlayer.IsMasterClient && !oneTimeSend)
        {
            oneTimeSend = true;

            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentNumberOfPlayers <= 3)
            {
                GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker = 6;
            }

            PhotonNetwork.LoadLevel("Game Level Transition");

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
    void WheelResult(string player, string wheelRes)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = wheelRes + "!";
            GameObject.Find(player).GetComponent<PlayerResultScript>().winnerVoting = true; 
            GameObject.Find(player).GetComponent<PlayerResultScript>().playerSabotageChoice = false;
            GameObject.Find(player).GetComponent<PlayerResultScript>().wheelResult = wheelRes;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void setVotedHead(string pname, int vote)
    {
        try
        {
            GameObject.Find(pname).GetComponent<PlayerResultScript>().sabotagedPlayerChoice = vote;
            GameObject.Find(pname).GetComponent<PlayerResultScript>().playerSabotageChoice = true;
            GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[sabotagedPlayerChoice].username + " Sabotaged!";
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().setPlayersSabotageStatus(true, vote);

            GameObject.Find(pname).GetComponent<PlayerResultScript>().winnersChoiceComplete = true;

        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void setSwitcharooDeadVote(string pname, int vote)
    {
        try
        {
            GameObject.Find(pname).GetComponent<PlayerResultScript>().switcherooDeadPlayer = vote;
            GameObject.Find(pname).GetComponent<PlayerResultScript>().deadPlayerChoice = true;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void setSwitcharooAliveVote(string pname, int voteAlive, int voteDead)
    {
        try
        {
            GameObject.Find(pname).GetComponent<PlayerResultScript>().switcherooAlivePlayer = voteAlive;

            GameObject.Find("Scene Manager").GetComponent<SceneManage>().setPlayersSwitcharooStatus(true, voteDead);
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().setPlayersSwitcharooStatus(false, voteAlive);

            GameObject.Find(pname).GetComponent<PlayerResultScript>().playerSabotageChoice = true;
            GameObject.Find(pname).GetComponent<PlayerResultScript>().winnersChoiceComplete = true;
            GameObject.Find("GameManager").GetComponent<ResultGameManager>().countdownIsRunning = true;
            //GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[sabotagedPlayerChoice].username + " Sabotaged!";
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void announceSabotagedVote(string vote)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<ResultGameManager>().wheelText.text = vote + " sabotaged!";
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

