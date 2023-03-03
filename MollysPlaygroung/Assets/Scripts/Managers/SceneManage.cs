using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class SceneManage : MonoBehaviour
{
    public int sceneTracker;

    public string[] levelNames;

    public List<string> allPlayersInGame = new List<string>();
    public List<string> allPlayersDead = new List<string>();

    public bool CurrentLevelState;
    public bool GameplayDone = false;

    public string MasterPlayer;

    // USER DEMO VARIABLES
    public bool SingleOrMultiPlayer = false;                                        // False = single player, True = multi player

    void Awake()
    {
        DontDestroyOnLoad(this);

        sceneTracker = 0;

        CurrentLevelState = false;

        SingleOrMultiPlayer = false;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            allPlayersInGame.Add(player.NickName);
        }
    }

    // Scene flow
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;                                // Syncing all players views once they're in a room
        SingleOrMultiPlayer = false;
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("BeginGame", 3);
            //StartCoroutine("TestGame", 2);
        }
    }

    // test
    IEnumerator TestGame(int value)
    {
        yield return new WaitForSeconds(value);

        PhotonNetwork.LoadLevel("Game Level Transition");

        yield return new WaitForSeconds(value);

        PhotonNetwork.LoadLevel("MovementTester");
    }

    public void NextGameaz()
    {
        GameplayDone = false;
        CurrentLevelState = false;

        // go to next level, single and multiplayer completed
        if (SingleOrMultiPlayer)
        {
            SingleOrMultiPlayer = !SingleOrMultiPlayer;

            sceneTracker++;

            if (sceneTracker == levelNames.Length)
            {

                PhotonNetwork.LoadLevel("Game Ending");

            }
            else
            {
                GameplayDone = false;

                PhotonNetwork.LoadLevel("Survey Reminder");
                //StartCoroutine("LevelTransition", 3);
            }
        }
        // finish single player
        else
        {
            SingleOrMultiPlayer = !SingleOrMultiPlayer;

            CurrentLevelState = false;
            GameplayDone = false;

            PhotonNetwork.LoadLevel("Game Level Transition");
           //StartCoroutine("LevelTransition", 3);
        }
    }


    public void NextSceneButton()
    {
        GameplayDone = false;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(levelNames[sceneTracker]);
        }
    }

    IEnumerator BeginGame(int value)
    {
        yield return new WaitForSeconds(value);

        PhotonNetwork.LoadLevel("Game Level Transition");
    }

    IEnumerator LevelTransition(int value)
    {
        PhotonNetwork.LoadLevel("Game Level Transition");

        GameplayDone = false;

        yield return new WaitForSeconds(value);

        PhotonNetwork.LoadLevel(levelNames[sceneTracker]);
    }

    void Update()
    {
        /*if (CurrentLevelState)
        {
            NextGameaz();
        }*/

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                try
                {
                    MasterPlayer = player.NickName;
                    //GameObject.Find(player.NickName).tag = "MasterClient";
                }
                catch (NullReferenceException e)
                {
                    // error
                }
            }
        }
    }

    // Players
    public void DeadPlayer(string name)
    {
        allPlayersDead.Add(name);

        for (int i = 0; 0 < allPlayersInGame.Count; i++)
        {
            if (allPlayersInGame[i] == name)
            {
                allPlayersInGame.RemoveAt(i);
                break;
            }
        }
    }
}
