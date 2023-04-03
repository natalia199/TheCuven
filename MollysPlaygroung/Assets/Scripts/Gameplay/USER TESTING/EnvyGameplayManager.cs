using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnvyGameplayManager : MonoBehaviour
{
    public Material chosenMesh;
    public List<Material> unchosenMesh = new List<Material>();

    public List<string> EnvyResults = new List<string>();
    public string loserPos;
    public List<GameObject> EnvyHorses = new List<GameObject>();

    public List<int> EnvyPointsResult = new List<int>();

    public GameObject scoreBoard;

    public bool votingSystem = true;
    public bool oneTimeRound = false;

    public int levelRounds = 0;

    public TextMeshProUGUI betTxt;

    public void RecordHorseResult(GameObject horse)
    {
        EnvyResults.Add(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[horse.GetComponent<EnvyHorse>().id].username);

        // FIRST PLAYER POINTS
        if (EnvyResults.Count == 1)
        {
            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
            {
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username == GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[horse.GetComponent<EnvyHorse>().id].username)
                {
                    EnvyPointsResult[x] += 2;
                    break;
                }
            }
        }
    }

    public void setLoser()
    {
        // LOSER POINTS DEDCUTION
        for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
        {
            for (int y = 0; y < EnvyResults.Count; y++)
            {
                // if current [x] game player is found in the list of winners - dont add to loser list and move on
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username == EnvyResults[y])
                {
                    break;
                }

                if (y >= (EnvyResults.Count - 1))
                {
                    EnvyResults.Add(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username);
                    loserPos = "" + x;
                    EnvyPointsResult[x] -= 1;
                    break;
                }
            }
        }
    }

    public void setPlayerPoints()
    {
        // Giving all correct guesses A POINT
        for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
        {
            // check if voted head of player x is the same number as the loser
            if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().votedHead == loserPos)
            {
                EnvyPointsResult[x] += 1;
            }
            
            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().votedHead = "";
        }

        votingSystem = true;
        EnvyResults = new List<string>();
        levelRounds++;
        loserPos = "";
        oneTimeRound = false;

        for (int i = 0; i < EnvyHorses.Count; i++)
        {
            EnvyHorses[i].GetComponent<EnvyHorse>().resetAll();
        }
    }

    // Getting horse and moving/stopping on track
    public void MoveHorseForward(string name)
    {
        GameObject.Find(name).GetComponent<EnvyHorse>().MoveYourHorse();
    }
    public void StopHorseForward(string name)
    {
        GameObject.Find(name).GetComponent<EnvyHorse>().StopYourHorse();
    }

    // Getting gun and activating/deactivating water squirt
    public void squirtWater(string name)
    {
        GameObject.Find(name).transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = true;
    }
    public void desquirtWater(string name)
    {
        GameObject.Find(name).transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = false;
    }
}
