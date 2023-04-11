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
    public int chosenLevelIndex;

    //public string[] levelNames;

    public List<string> minigameLevels = new List<string>();
    public string finalMinigame;


    public List<string> allPlayersInGame = new List<string>();
    public List<GamePlayer> playersInGame = new List<GamePlayer>();
    public List<string> allPlayersDead = new List<string>();

    public bool CurrentLevelState;
    public bool GameplayDone = false;

    public string MasterPlayer;

    public List<GameObject> characters = new List<GameObject>();
    public List<GameObject> modelledCharacters = new List<GameObject>();
    public Material winnerMesh;

    public GameObject usernameScene;
    public GameObject rumbleScene;
    public GameObject charSelScene;

    public string currentState = "username";

    bool gameFlowBegin = false;

    public List<string> levelsLoser = new List<string>();
    public List<string> levelsWinner = new List<string>();

    public Material deadSkin;

    public bool countdownLevelCheck = true;

    public bool beginGame = false;

    public bool LastLevelPride = false;

    public struct GamePlayer
    {
        public string username;
        public string chosenCharacter;
        public int characterID;
        public bool stillAlive;
        public Material originalMesh;        
        public bool variablesSet;
        public int deathTracker;
        public bool sabotaged;
    };

    public void createPlayerStruct(string name)
    {
        GamePlayer boy;
        boy.username = name;
        boy.chosenCharacter = "";
        boy.characterID = -1;
        boy.stillAlive = false;
        boy.originalMesh = null;
        boy.variablesSet = false;
        boy.deathTracker = 0;
        boy.sabotaged = false;
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
                die.characterID = y;
                break;
            }
        }

        Debug.Log("die.characterID " + die.characterID);

        GameObject.Find(pname).transform.GetChild(2).GetChild(die.characterID).gameObject.SetActive(true);
        die.originalMesh = GameObject.Find(pname).transform.GetChild(2).GetChild(die.characterID).GetChild(1).GetComponent<SkinnedMeshRenderer>().material;

        die.stillAlive = true;
        die.variablesSet = true;
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

        sceneTracker = 0;
        chosenLevelIndex = UnityEngine.Random.Range(0, minigameLevels.Count);

        CurrentLevelState = false;
        countdownLevelCheck = true;

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

        usernameScene.SetActive(true);
        rumbleScene.SetActive(false);
        charSelScene.SetActive(false);
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

        else if (SceneManager.GetActiveScene().name == "Intro_Scene" && !gameFlowBegin)
        {
            if (playersInGame.Count == 2)
            {
                LastLevelPride = true;
            }


            countdownLevelCheck = false;

            if (PhotonNetwork.IsMasterClient)
            {
                //StartCoroutine("BeginGame", 3);
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


    public void NextGameaz()
    {
        GameplayDone = false;
        CurrentLevelState = false;

        sceneTracker++;

        if (sceneTracker == (playersInGame.Count - 2))
        {
            LastLevelPride = true;
            GameplayDone = false;
            PhotonNetwork.LoadLevel("LevelResult");
        }
        else if (sceneTracker == (playersInGame.Count - 1))
        {
            PhotonNetwork.LoadLevel("Game Ending");
        }
        else
        {
            GameplayDone = false;
            PhotonNetwork.LoadLevel("LevelResult");
        }
    }


    public void NextSceneButton()
    {
        GameplayDone = false;

        if (PhotonNetwork.IsMasterClient)
        {
            if (!LastLevelPride)
            {
                PhotonNetwork.LoadLevel(minigameLevels[sceneTracker]);
            }
            else
            {
                PhotonNetwork.LoadLevel(finalMinigame);
            }
        }
    }

    IEnumerator BeginGame(int value)
    {
        yield return new WaitForSeconds(value);

        PhotonNetwork.LoadLevel("Game Level Transition");
    }

    public void currentLevelsLoser(string n)
    {
        if (levelsLoser.Count == 0)
        {
            levelsLoser.Add(n);
        }
        else 
        { 
            for (int y = 0; y < levelsLoser.Count; y++)
            {
                if (n == levelsLoser[y])
                {
                    break;
                }

                if (y >= (levelsLoser.Count - 1))
                {
                    // saves all players that died in the level
                    levelsLoser.Add(n);
                }
            }
        }
    }

    public void setPlayersLifeStatus(bool state)
    {
        for (int x = 0; x < playersInGame.Count; x++)
        {
            if (levelsLoser[0] == playersInGame[x].username)
            {
                GamePlayer die = playersInGame[x];
                die.stillAlive = state;
                die.deathTracker++;
                //Debug.Log(die.username + " death " + die.deathTracker);
                playersInGame[x] = die;
            }
        }
    }

    public void currentLevelsWinner(string n)
    {
        // saves all players that died in the level
        levelsWinner.Add(n);
    }

    public void setPlayersSabotageStatus(bool state, int player)
    {
        GamePlayer die = playersInGame[player];
        die.sabotaged = state;
        playersInGame[player] = die;
    }
    
    public void setPlayersSwitcharooStatus(bool state, int player)
    {
        GamePlayer die = playersInGame[player];
        die.stillAlive = state;
        playersInGame[player] = die;
    }
}
