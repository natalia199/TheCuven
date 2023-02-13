using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SceneManage : MonoBehaviour
{
    public int sceneTracker;

    public string[] levelNames = { "Lust", "Gluttony", "Greed", "Sloth", "Wrath", "Envy", "Pride" };

    public List<string> allPlayersInGame = new List<string>();
    public List<string> allPlayersDead = new List<string>();

    public bool CurrentLevelState;

    void Awake()
    {
        DontDestroyOnLoad(this);

        sceneTracker = 0;

        CurrentLevelState = false;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            allPlayersInGame.Add(player.NickName);
        }
    }

    // Scene flow
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;                                // Syncing all players views once they're in a room

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
        if (PhotonNetwork.IsMasterClient)
        {
            sceneTracker++;
            CurrentLevelState = false;

            if (sceneTracker == levelNames.Length)
            {
                PhotonNetwork.LoadLevel("Game Ending");
            }
            else
            {
                StartCoroutine("LevelTransition", 3);
            }
        }
    }

    IEnumerator BeginGame(int value)
    {
        yield return new WaitForSeconds(value);

        PhotonNetwork.LoadLevel("Game Level Transition");

        yield return new WaitForSeconds(value);

        PhotonNetwork.LoadLevel(levelNames[0]);
    }

    IEnumerator LevelTransition(int value)
    {
        PhotonNetwork.LoadLevel("Game Level Transition");

        yield return new WaitForSeconds(value);

        PhotonNetwork.LoadLevel(levelNames[sceneTracker]);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && CurrentLevelState)
        {
            NextGameaz();
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
