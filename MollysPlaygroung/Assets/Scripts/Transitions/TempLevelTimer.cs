using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class TempLevelTimer : MonoBehaviour
{
    //public GameObject resultSlots;
    //public GameObject bgImg;

    public float timeRemaining;
    public float startingCountdown;
    public bool countdownIsRunning = false;
    public bool timerIsRunning = false;
    //public TextMeshProUGUI timeText;
    public TextMeshProUGUI countdownText;

    public TextMeshProUGUI finishedMsg;

    public bool oneTimeRun = false;

    List<int> playerLevelIDs = new List<int>();
    List<int> chipValues = new List<int>();
    List<int> keyValues = new List<int>();


    void Start()
    {
        // Starts the timer automatically
        //timeRemaining = 0;
        startingCountdown = 4;
        //timerIsRunning = true;
        countdownIsRunning = true;
        oneTimeRun = false;
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().countdownLevelCheck = false;
        //resultSlots.SetActive(false);
    }

    void Update()
    {
        /*
        if (timerIsRunning)
        {
            timeText.text = "" + Mathf.FloorToInt(timeRemaining);
            timeRemaining += Time.deltaTime;
        }
        else
        {
            timeText.text = "";
        }
        */

        if (countdownIsRunning)
        {
            if (startingCountdown > 1)
            {
                countdownText.text = "" + Mathf.FloorToInt(startingCountdown);
            }
            else
            {
                countdownText.text = "START";
            }

            startingCountdown -= Time.deltaTime;

            if (startingCountdown < 0)
            {
                countdownIsRunning = false;
                GameObject.Find("Scene Manager").GetComponent<SceneManage>().countdownLevelCheck = true;
            }
        }
        else
        {
            countdownText.text = "";
        }
    }

    public void CallGameEnd()
    {
        timerIsRunning = false;

        if (!oneTimeRun)
        {
            try
            {
                StartCoroutine("DisplayerFinalResults");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.Log("rude");
            }
        }
    }

    IEnumerator DisplayerFinalResults()
    {
        oneTimeRun = true;

        playerLevelIDs = new List<int>();

        if (SceneManager.GetActiveScene().name == "Wrath" || SceneManager.GetActiveScene().name == "Sloth" || SceneManager.GetActiveScene().name == "Gluttony" || SceneManager.GetActiveScene().name == "Pride")
        {
            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
            {
                for (int y = 0; y < GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsLoser.Count; y++)
                {
                    // if current [x] game player is found in the list of losers - dont add to winner list and move on
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username == GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsLoser[y])
                    {
                        break;
                    }

                    if (y >= (GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsLoser.Count - 1))
                    {
                        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsWinner(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username);
                        break;
                    }
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Envy")
        {
            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
            {
                for (int y = 0; y < GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner.Count; y++)
                {
                    // if current [x] game player is found in the list of winners - dont add to loser list and move on
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username == GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[y])
                    {
                        break;
                    }

                    if (y >= (GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner.Count - 1))
                    {
                        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username);
                        break;
                    }
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Greed")
        {
            for (int y = 0; y < GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones.Count; y++)
            {
                if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().activatedForPlayer)
                {
                    if (y == 0)
                    {
                        chipValues.Add(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[0].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone);
                        playerLevelIDs.Add(y);
                    }
                    else
                    {
                        if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone >= chipValues[0])
                        {
                            Debug.Log("place infront");
                            chipValues.Insert(0, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone);
                            playerLevelIDs.Insert(0, y);
                        }
                        else if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone <= chipValues[chipValues.Count - 1])
                        {
                            Debug.Log("add to the end");
                            chipValues.Add(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone);
                            playerLevelIDs.Add(y);
                        }
                    }
                }

                if (y == (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones.Count - 1))
                {
                    GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsWinner(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[0]].username);
                    GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[playerLevelIDs.Count - 1]].username);
                }
            }

        }
        else if (SceneManager.GetActiveScene().name == "Lust")
        {
            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
            {
                if (x == 0)
                {
                    keyValues.Add(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys);
                    playerLevelIDs.Add(x);
                }
                else
                {
                    if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys >= keyValues[0])
                    {
                        keyValues.Insert(0, GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys);
                        playerLevelIDs.Insert(0, x);
                    }
                    else if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys <= keyValues[keyValues.Count - 1])
                    {
                        keyValues.Add(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys);
                        playerLevelIDs.Add(x);
                    }


                    if (x == (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count - 1))
                    {
                        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsWinner(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[0]].username);
                        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[playerLevelIDs.Count - 1]].username);
                    }
                }
            }

            GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;

            finishedMsg.text = "FINISHED";
            finishedMsg.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "FINISHED";

            yield return new WaitForSeconds(3f);

            GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextGameaz();
        }

        /*
        void DisplayTime(float timeToDisplay)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        */
    }
}
