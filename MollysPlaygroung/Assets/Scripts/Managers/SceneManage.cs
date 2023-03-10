using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public int sceneTracker;

    public string[] levelNames;

    public List<string> allPlayersInGame = new List<string>();
    public List<GamePlayer> playersInGame = new List<GamePlayer>();
    public List<string> allPlayersDead = new List<string>();

    public bool CurrentLevelState;
    public bool GameplayDone = false;

    public string MasterPlayer;

    // USER DEMO VARIABLES
    //public bool SingleOrMultiPlayer = true;                                        // False = single player, True = multi player

    public struct GamePlayer
    {
        public string username;
        public string chosenCharacter;
        public int listID;
        public bool stillAlive;
    };

    public void createPlayerStruct(string name)
    {
        GamePlayer boy;
        boy.username = name;
        boy.chosenCharacter = "";
        boy.listID = -1;
        boy.stillAlive = true;
        playersInGame.Add(boy);
    }

    public void updatePlayerCharacter(string pname, string character, int x)
    {
        GamePlayer die = playersInGame[x];
        die.chosenCharacter = character;
        playersInGame[x] = die;

        if(PhotonNetwork.LocalPlayer.NickName == pname)
        {
            SceneManager.LoadScene("PlayerRumble");

        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);

        sceneTracker = 0;

        CurrentLevelState = false;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName.Length > 1)
            {
            }
            else
            {
                player.NickName = "oogabooga";
            }
        }

        //SingleOrMultiPlayer = true;

        /*
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            allPlayersInGame.Add(player.NickName);
        }
        */
    }

    // Scene flow
    void Start()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;                                // Syncing all players views once they're in a room
        //SingleOrMultiPlayer = true;
        
        /*if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("BeginGame", 3);
            //StartCoroutine("TestGame", 2);
        }
        */
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Username" || SceneManager.GetActiveScene().name == "Player Selection" || SceneManager.GetActiveScene().name == "PlayerRumble")
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.NickName.Length > 1)
                {
                    // love
                }
                else
                {
                    player.NickName = "oogabooga";
                }
            }

            // Adding player
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (playersInGame.Count > 0)
                {
                    bool notNeeded = false;

                    for (int i = 0; i < playersInGame.Count; i++)
                    {
                        if (player.NickName == playersInGame[i].username || player.NickName == "oogabooga")
                        {
                            notNeeded = true;
                            break;
                        }
                    }

                    if (!notNeeded)
                    {
                        Debug.Log("Adding " + player.NickName);
                        allPlayersInGame.Add(player.NickName);
                        createPlayerStruct(player.NickName);
                    }
                }
                else
                {
                    if (player.NickName != "oogabooga")
                    {
                        Debug.Log("Adding " + player.NickName);
                        allPlayersInGame.Add(player.NickName);
                        createPlayerStruct(player.NickName);
                        break;
                    }
                }
            }

            // Removing non-existing players
            for (int x = 0; x < playersInGame.Count; x++)
            {
                bool dontExist = false;

                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (playersInGame[x].username == PhotonNetwork.PlayerList[i].NickName || playersInGame[x].username == "oogabooga")
                    {
                        dontExist = true;
                        break;
                    }
                }

                if (!dontExist)
                {
                    Debug.Log("removing " + playersInGame[x].username);
                    allPlayersInGame.RemoveAt(x);
                    playersInGame.RemoveAt(x);
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "PlayerRumble")
        {
            foreach (GamePlayer player in playersInGame)
            {
                Debug.Log("Player " + player.username + " picked " + player.chosenCharacter);

            }
            /*if (CurrentLevelState)
            {
                NextGameaz();
            }*/

            /*if (SceneManager.GetActiveScene().name == "Username") 
            {
                boy.username = player.NickName;            
            }


            foreach (Player player in PhotonNetwork.PlayerList)
            {
                allPlayersInGame.Add(player.NickName);

                GamePlayer boy;
                boy.username = player.NickName;
                boy.chosenCharacter = "";
                boy.listID = -1;
                boy.stillAlive = true;
                playersInGame.Add(boy);
            }
            */
        }

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

        sceneTracker++;

        if (sceneTracker == levelNames.Length)
        {

            PhotonNetwork.LoadLevel("Game Ending");

        }
        else
        {
            GameplayDone = false;

            //PhotonNetwork.LoadLevel("Survey Reminder");
            //StartCoroutine("LevelTransition", 3);
            PhotonNetwork.LoadLevel("Game Level Transition");

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
