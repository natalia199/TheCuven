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

    // Moving platform


    void Start()
    {
        grrr();

        for (int i = 0; i < playingPlayers.Count; i++)
        {
            playingPlayersStats.Add(false);
        }
    }

    void Update()
    {

    }

    void grrr()
    {
        List<string> tempPlayers = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame;

        for (int i = 0; i < tempPlayers.Count; i++)
        {
            playingPlayers.Add(tempPlayers[i]);
        }
    }



    // PROGRESS ONE CODE
    /*
    public List<string> playingPlayers = new List<string>();
    public List<bool> playingPlayersStats = new List<bool>();

    public List<string> raceResults = new List<string>();

    public GameObject winningCircle;
    public GameObject resultScreen;

    public bool gameover;
    public string wrathWinner;

    void Start()
    {
        gameover = false;

        grrr();

        //resultScreen.SetActive(false);

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
            //resultScreen.SetActive(true);
            if (!gameover)
            {
                gameover = true;
                PostResults();
            }
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
            winningCircle.transform.GetChild(results.Count - i).gameObject.SetActive(true);
            winningCircle.transform.GetChild(results.Count - i).GetComponent<TextMeshProUGUI>().text = winningCircle.transform.GetChild(results.Count - i).GetComponent<TextMeshProUGUI>().text + results[i];            
        }

        for (int x = 0; x < playingPlayers.Count; x++)
        {
            for (int y = 0; y < results.Count; y++)
            {
                if (results[y] == playingPlayers[x])
                {
                    break;
                }
                else
                {
                    wrathWinner = playingPlayers[x];
                    break;
                }
            }
        }

        winningCircle.transform.GetChild(0).gameObject.SetActive(true);
        winningCircle.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        winningCircle.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = winningCircle.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text + wrathWinner;

        winningCircle.transform.GetChild(playingPlayers.Count - 1).GetChild(1).gameObject.SetActive(true);
    }
    */
}
