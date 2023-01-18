using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class EnvyGameManager : MonoBehaviour
{
    //public List<GameObject> playingPlayers = new List<GameObject>();
    public List<string> playingPlayers = new List<string>();
    public List<GameObject> playerInfoStat = new List<GameObject>();

    // voting shit
    public GameObject voteIDs;
    public GameObject voteNames;

    public List<bool> playingPlayersStats = new List<bool>();
    public List<GameObject> raceHorses = new List<GameObject>();

    public List<string> raceResults = new List<string>();

    public TextMeshProUGUI horseDisplay;

    public bool votingSystem = true;
    public GameObject votingNames;
    public GameObject votingScreen;
    public GameObject carnivalScreen;

    bool oneAndDone = false;

    void Start()
    {
        horseDisplay = GameObject.Find("HorseNamies").GetComponent<TextMeshProUGUI>();

        grrr();

        carnivalScreen.SetActive(false);


        for (int i = 0; i < playingPlayers.Count; i++)
        {
            playingPlayersStats.Add(false);
        }
        //SetPlayerHorses();
    }

    void Update()
    {
        if (!checkPlayerVadility()) {
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
        else if(!oneAndDone && !votingSystem)
        {
            for (int i = 0; i < playingPlayers.Count; i++)
            {
                GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().AssigningHorses();
            }

            for (int i = 0; i < playingPlayers.Count; i++)
            {
                playerInfoStat[i].SetActive(true);
                playerInfoStat[i].GetComponent<TextMeshProUGUI>().text = playingPlayers[i];
                playerInfoStat[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().horseName;
                playerInfoStat[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().votedPlayerName;
            }

            oneAndDone = true;
        }

        if (checkPlayerVadility() && !votingSystem)
        {
            votingScreen.SetActive(false);
            votingNames.SetActive(false);
            carnivalScreen.SetActive(true);
        }
        else if (checkPlayerVadility())
        {
            votingSystem = CheckForAllVotes();
        }

    }

    public string SendingVoterName(string ID)
    {
        for(int i = 0; i < voteNames.transform.childCount; i++)
        {
            if(voteIDs.transform.GetChild(i).name == ID)
            {
                return voteNames.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text;
            }
        }

        return "invalid";
    }

    bool CheckForAllVotes()
    {
        for (int i = 0; i < playingPlayers.Count; i++)
        {
            if(GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().votedFor == "0")
            {
                return true;
            }
        }

        return false;
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

    void grrr()
    {
        List<string> tempPlayers = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame;

        for (int i = 0; i < tempPlayers.Count; i++)
        {
            playingPlayers.Add(tempPlayers[i]);
        }

        for (int i = 0; i < playingPlayers.Count; i++)
        {
            if (playingPlayers[i] != PhotonNetwork.LocalPlayer.NickName)
            {
                try
                {
                    voteIDs.transform.GetChild(i).gameObject.SetActive(true);
                    voteNames.transform.GetChild(i).gameObject.SetActive(true);

                    voteNames.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = playingPlayers[i];
                    GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().votingCardID = voteIDs.transform.GetChild(i).gameObject.name;
                }
                catch (NullReferenceException e)
                {
                    // error
                }
            }
        }
    }

    public void SetPlayerHorses()
    {
        List<string> temp = new List<string>();
        int index = 0;

        for (int i = 0; i < raceHorses.Count; i++)
        {
            temp.Add(raceHorses[i].name);
        }

        for (int i = 0; i < playingPlayers.Count; i++)
        {
            for (int y = 0; y < temp.Count; y++)
            {
                Debug.Log(temp[y]);
            }

            //int index = Random.Range(0, (temp.Count-1));
            GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().horseName = temp[index];
            playerInfoStat[i].SetActive(true);
            playerInfoStat[i].GetComponent<TextMeshProUGUI>().text = playingPlayers[i];
            playerInfoStat[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp[index];
            GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().infoz = playerInfoStat[i];

            horseDisplay.text = "#" + GameObject.Find(temp[index]).transform.GetChild(0).GetComponent<HorseFinishLine>().horseID + " - " + temp[index];

            // attach player info g.o
            temp.RemoveAt(index);
        }
    }

    public void MoveHorse(string horse, Vector3 position)
    {
        GameObject.Find(horse).transform.GetChild(0).transform.position = position;
    }

    public void UpdateRaceResult(string name)
    {        
        raceResults.Add(name);
    }
}
