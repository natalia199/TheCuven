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

    public List<GameObject> characters = new List<GameObject>();
    public List<GameObject> modelledCharacters = new List<GameObject>();

    public GameObject usernameScene;
    public GameObject rumbleScene;
    public GameObject charSelScene;

    public string currentState = "username";

    bool gameFlowBegin = false;

    public List<string> levelsLoser = new List<string>();

    public Material deadSkin;

    // USER DEMO VARIABLES
    //public bool SingleOrMultiPlayer = true;                                        // False = single player, True = multi player

    public struct GamePlayer
    {
        public string username;
        public string chosenCharacter;
        public int listID;
        public bool stillAlive;
        public Material originalMesh;
    };

    public void createPlayerStruct(string name)
    {
        GamePlayer boy;
        boy.username = name;
        boy.chosenCharacter = "";
        boy.listID = -1;
        boy.stillAlive = false;
        boy.originalMesh = null;
        //boy.mesh = null;
        playersInGame.Add(boy);
    }

    public void updatePlayerCharacter(string pname, string character, int x)
    {
        GamePlayer die = playersInGame[x];
        die.chosenCharacter = character;

        GameObject.Find(pname).transform.GetChild(2).gameObject.SetActive(true);

        for (int y = 0; y < GameObject.Find(pname).transform.GetChild(2).childCount; y++)
        {
            if (GameObject.Find(pname).transform.GetChild(2).GetChild(y).name == character)
            {
                GameObject.Find(pname).transform.GetChild(2).GetChild(y).gameObject.SetActive(true);
                die.originalMesh = GameObject.Find(pname).transform.GetChild(2).GetChild(y).GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
            }
            else
            {
                Destroy(GameObject.Find(pname).transform.GetChild(2).GetChild(y).gameObject);                
            }
        }
            
        die.stillAlive = true;
        playersInGame[x] = die;

        GameObject.Find(pname).layer = 6;
        //GameObject.Find(pname).transform.GetChild(2).gameObject.SetActive(true);
    }

    public void switchCamera(bool state)
    {
        if (!state)
        {
            usernameScene.SetActive(false);
            rumbleScene.SetActive(false);
            charSelScene.SetActive(true);

            currentState = "characterSelection";
        }
        else
        {
            usernameScene.SetActive(false);
            rumbleScene.SetActive(true);
            charSelScene.SetActive(false);

            currentState = "rumble";
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);

        usernameScene.SetActive(true);
        rumbleScene.SetActive(false);
        charSelScene.SetActive(false);

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
    }

    // Scene flow
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;                                // Syncing all players views once they're in a room

        /*if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("BeginGame", 3);
            //StartCoroutine("TestGame", 2);
        }
        */

        Debug.Log("start 1");
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

        else if (SceneManager.GetActiveScene().name == "Game Introduction" && !gameFlowBegin)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine("BeginGame", 3);
            }
            gameFlowBegin = true;
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

        /*
        if (sceneTracker == levelNames.Length)
        {

            PhotonNetwork.LoadLevel("Game Ending");

        }
        else
        {
        */
            GameplayDone = false;

            PhotonNetwork.LoadLevel("LevelResult");

       // }
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

    public void currentLevelsLoser(string n)
    {
        if (levelsLoser.Count == 0)
        {
            for (int x = 0; x < playersInGame.Count; x++)
            {
                if (n == playersInGame[x].username && playersInGame[x].stillAlive)
                {
                    levelsLoser.Add(n);
                    break;
                }
            }
        }
    }

    public void setPlayersLifeStatus(bool state)
    {
        for (int x = 0; x < playersInGame.Count; x++)
        {
            if (levelsLoser[0] ==playersInGame[x].username)
            {
                GamePlayer die = playersInGame[x];
                die.stillAlive = state;
                playersInGame[x] = die;
            }
        }
    }
}
