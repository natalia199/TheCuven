using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

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

    public bool votingSystem = true;
    public bool gameover = false;
    public GameObject votingNames;
    public GameObject votingScreen;
    public GameObject carnivalScreen;
    public GameObject resultScreen;
    public GameObject winningCircle;

    bool oneAndDone = false;

    TextMeshProUGUI horseDisplay;

    public string ownersHorse;
    public string currentOwner;

    public string loser;
    public List<int> maxPts = new List<int>();
    public List<int> minPts = new List<int>();

    void Start()
    {
        grrr();

        carnivalScreen.SetActive(false);
        resultScreen.SetActive(false);


        for (int i = 0; i < playingPlayers.Count; i++)
        {
            playingPlayersStats.Add(false);
        }
        //SetPlayerHorses();

        currentOwner = PhotonNetwork.LocalPlayer.NickName;
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

        if (raceResults.Count == (playingPlayers.Count - 1) && !votingSystem)
        {
            carnivalScreen.SetActive(false);
            resultScreen.SetActive(true);

            if (!gameover)
            {
                PostResults();
                StartCoroutine("WaitBeforeGameChange", 10);
            }
            Debug.Log("GAMEOVER");
            votingSystem = true;
        }
        else if (checkPlayerVadility() && !votingSystem)
        {
            votingScreen.SetActive(false);
            votingNames.SetActive(false);
            carnivalScreen.SetActive(true);

            try
            {
                GameObject.Find("HorseNamies").GetComponent<TextMeshProUGUI>().text = "#" + GameObject.Find(GameObject.Find(currentOwner).GetComponent<PlayerEnvy>().horseName).transform.GetChild(0).GetComponent<HorseFinishLine>().horseID + " " + GameObject.Find(currentOwner).GetComponent<PlayerEnvy>().horseName;
            }
            catch (NullReferenceException e)
            {
                // error
            }
        }
        else if (checkPlayerVadility() && raceResults.Count == 0)
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

    void PostResults()
    {
        List<string> players = new List<string>(playingPlayers);
        List<string> results = new List<string>(raceResults);
        List<string> placements = new List<string>();

        for(int i = 0; i < results.Count; i++)
        {
            for(int y = 0; y < players.Count; y++)
            {
                if(results[i] == GameObject.Find(players[y]).GetComponent<PlayerEnvy>().horseName)
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

        List<int> points = new List<int>();
        for (int x = 0; x < placements.Count; x++)
        {
            int pts = PointSystem(placements[x], x + 1, placements[placements.Count-1]);
            points.Add(pts);
            winningCircle.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().text = pts + "";
        }

        int max = points[0];
        int min = points[0];

        for (int x = 0; x < points.Count; x++)
        {
            for (int y = 0; y < points.Count; y++)
            {
                if (points[x] < points[y])
                {
                    max = points[y];
                }
                if(points[x] > points[y])
                {
                    min = points[y];
                }
            }
        }

        for (int x = 0; x < winningCircle.transform.childCount; x++)
        {
            if (winningCircle.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().text == (max + ""))
            {
                winningCircle.transform.GetChild(x).GetChild(1).gameObject.SetActive(true);
            }
            else if (winningCircle.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().text == (min + ""))
            {
                winningCircle.transform.GetChild(x).GetChild(2).gameObject.SetActive(true);
            }
        }

        //int max = Convert.ToInt32(winningCircle.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text);

        /*
        Debug.Log("final max is " + max);

        int min = -Convert.ToInt32(winningCircle.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text);
        for (int x = 0; x < winningCircle.transform.childCount; x++)
        {
            for (int y = 0; y < winningCircle.transform.childCount; y++)
            {
                if (Convert.ToInt32(winningCircle.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().text) > Convert.ToInt32(winningCircle.transform.GetChild(y).GetChild(0).GetComponent<TextMeshProUGUI>().text))
                {
                    min = Convert.ToInt32(winningCircle.transform.GetChild(y).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                    Debug.Log("min is " + min);

                }
            }
        }

        Debug.Log("final min is " + min);
        */

        /*
    int max_num = points.AsQueryable().Max();
    int min_num = points.AsQueryable().Min();

    for (int x = 0; x < winningCircle.transform.childCount; x++)
    {
        if(winningCircle.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().text == (max_num + ""))
        {
            winningCircle.transform.GetChild(x).GetChild(1).gameObject.SetActive(true);
        }
        else if (winningCircle.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().text == (min_num + ""))
        {
            winningCircle.transform.GetChild(x).GetChild(2).gameObject.SetActive(true);
        }
    }
        */
        loser = players[0];

        gameover = true;
    }

    int PointSystem(string player, int place, string loser)
    {
        int totalPts = 0;

        if(place == 1)
        {
            totalPts++;
        }

        if(place == playingPlayers.Count)
        {
            totalPts--;
        }

        if(GameObject.Find(player).GetComponent<PlayerEnvy>().votedPlayerName == loser)
        {
            totalPts++;
        }

        return totalPts;
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

            //GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().endScale = GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().orgScale;

            playerInfoStat[i].SetActive(true);
            playerInfoStat[i].GetComponent<TextMeshProUGUI>().text = playingPlayers[i];
            playerInfoStat[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp[index];
            GameObject.Find(playingPlayers[i]).GetComponent<PlayerEnvy>().infoz = playerInfoStat[i];

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

    IEnumerator WaitBeforeGameChange(int value)
    {
        Debug.Log("moment of silence for " + loser);

        //GameObject.Find("Scene Manager").GetComponent<SceneManage>().DeadPlayer(loser);

        yield return new WaitForSeconds(value);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextGameaz();
    }
}
