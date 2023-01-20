using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;

public class WrathGameManager : MonoBehaviour
{
    public List<string> playingPlayers = new List<string>();
    public List<bool> playingPlayersStats = new List<bool>();

    public List<string> raceResults = new List<string>();

    public GameObject winningCircle;
    public GameObject resultScreen;

    public bool gameover;

    void Start()
    {
        gameover = false;

        grrr();

        resultScreen.SetActive(false);

        for (int i = 0; i < playingPlayers.Count; i++)
        {
            playingPlayersStats.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkPlayerVadility())
        {
            for (int i = 0; i < playingPlayers.Count; i++)
            {
                try
                {
                    if (GameObject.Find(playingPlayers[i]).scene.IsValid())
                    {
                        playingPlayersStats[i] = true;
                    }
                }
                catch (NullReferenceException e)
                {
                    // error
                }
            }
        }

        if (raceResults.Count == (playingPlayers.Count - 1))
        {
            resultScreen.SetActive(true);
            if (!gameover)
            {
                //PostResults();
            }
            Debug.Log("GAMEOVER");
        }
    }

    public void LoserList(string name)
    {
        raceResults.Add(name);
    }

    void grrr()
    {
        List<string> tempPlayers = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame;

        for (int i = 0; i < tempPlayers.Count; i++)
        {
            playingPlayers.Add(tempPlayers[i]);
        }
    }

    bool checkPlayerVadility()
    {
        for (int i = 0; i < playingPlayersStats.Count; i++)
        {
            if (!playingPlayersStats[i])
            {
                return false;
            }
        }

        return true;
    }

    void PostResults()
    {
        List<string> players = new List<string>(playingPlayers);
        List<string> results = new List<string>(raceResults);
        List<string> placements = new List<string>();

        for (int i = 0; i < results.Count; i++)
        {
            for (int y = 0; y < players.Count; y++)
            {
                if (results[i] == GameObject.Find(players[y]).GetComponent<PlayerEnvy>().horseName)
                {
                    winningCircle.transform.GetChild(i).gameObject.SetActive(true);
                    winningCircle.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = winningCircle.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text + players[y];
                    placements.Add(players[y]);
                    players.RemoveAt(y);
                    break;
                }
            }
        }

        winningCircle.transform.GetChild(results.Count).gameObject.SetActive(true);
        winningCircle.transform.GetChild(results.Count).GetComponent<TextMeshProUGUI>().text = winningCircle.transform.GetChild(results.Count).GetComponent<TextMeshProUGUI>().text + players[0];
        placements.Add(players[0]);

        /*
        for (int x = 0; x < winningCircle.transform.childCount; x++)
        {
            if (winningCircle.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().text == (max_num + ""))
            {
                winningCircle.transform.GetChild(x).GetChild(1).gameObject.SetActive(true);
            }
            else if (winningCircle.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().text == (min_num + ""))
            {
                winningCircle.transform.GetChild(x).GetChild(2).gameObject.SetActive(true);
            }
        }
        */

        gameover = true;
    }
}
