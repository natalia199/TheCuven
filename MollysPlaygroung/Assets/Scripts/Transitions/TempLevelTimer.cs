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
    List<int> bettingValues = new List<int>();


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
            bool grr = false;

            for (int y = 0; y < GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyPointsResult.Count; y++)
            {
                // Organizing list in ranking

                if (y == 0)
                {
                    bettingValues.Add(GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyPointsResult[0]);
                    playerLevelIDs.Add(y);
                }
                else
                {
                    // checking for top value
                    for (int z = 0; z < bettingValues.Count; z++)
                    {
                        if (GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyPointsResult[y] >= bettingValues[z])
                        {
                            bettingValues.Insert(z, GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyPointsResult[y]);
                            playerLevelIDs.Insert(z, y);
                            grr = true;
                            break;
                        }
                    }
                    if (!grr)
                    {
                        // checking for lowest value
                        for (int z = (bettingValues.Count - 1); z >= 0; z--)
                        {
                            if (GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyPointsResult[y] <= bettingValues[z])
                            {
                                // if last position
                                if (z == (bettingValues.Count - 1))
                                {
                                    bettingValues.Add(GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyPointsResult[y]);
                                    playerLevelIDs.Add(y);
                                    break;
                                }
                                else
                                {
                                    bettingValues.Insert(z + 1, GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyPointsResult[y]);
                                    playerLevelIDs.Insert(z + 1, y);
                                    break;
                                }
                            }
                        }
                    }

                    grr = false;
                }
            }

            GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsWinner(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[0]].username);
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[playerLevelIDs.Count - 1]].username);
        }
        else if (SceneManager.GetActiveScene().name == "Greed")
        {
            bool grr = false;

            for (int y = 0; y < GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones.Count; y++)
            {
                // Organizing list in ranking

                if (y == 0)
                {
                    chipValues.Add(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[0].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone);
                    playerLevelIDs.Add(y);
                }
                else
                {
                    // checking for top value
                    for (int z = 0; z < chipValues.Count; z++)
                    {
                        if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone >= chipValues[z])
                        {
                            chipValues.Insert(z, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone);
                            playerLevelIDs.Insert(z, y);
                            grr = true;
                            break;
                        }
                    }
                    if (!grr)
                    {
                        // checking for lowest value
                        for (int z = (chipValues.Count - 1); z >= 0; z--)
                        {
                            if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone <= chipValues[z])
                            {
                                // if last position
                                if (z == (chipValues.Count - 1))
                                {
                                    chipValues.Add(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone);
                                    playerLevelIDs.Add(y);
                                    break;
                                }
                                else
                                {
                                    chipValues.Insert(z + 1, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[y].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone);
                                    playerLevelIDs.Insert(z + 1, y);
                                    break;
                                }
                            }
                        }
                    }

                    grr = false;
                }
            }

            GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsWinner(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[0]].username);
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[playerLevelIDs.Count - 1]].username);
        }
        else if (SceneManager.GetActiveScene().name == "Lust")
        {
            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
            {

                bool grr = false;


                if (x == 0)
                {
                    keyValues.Add(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys);
                    playerLevelIDs.Add(x);
                }
                else
                {
                    // checking for top value
                    for (int z = 0; z < keyValues.Count; z++)
                    {
                        if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys >= keyValues[z])
                        {
                            keyValues.Insert(z, GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys);
                            playerLevelIDs.Insert(z, x);
                            grr = true;
                            break;
                        }
                    }
                    if (!grr)
                    {
                        // checking for lowest value
                        for (int z = (keyValues.Count - 1); z >= 0; z--)
                        {
                            if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys <= keyValues[z])
                            {
                                // if last position
                                if (z == (keyValues.Count - 1))
                                {
                                    keyValues.Add(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys);
                                    playerLevelIDs.Add(x);
                                    break;
                                }
                                else
                                    keyValues.Insert(z + 1, GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).GetComponent<PlayerUserTest>().hitKeys);
                                playerLevelIDs.Insert(z + 1, x);
                                break;
                            }


                        }
                    }

                    grr = false;

                }

            }


        }



        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsWinner(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[0]].username);
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[playerLevelIDs[playerLevelIDs.Count - 1]].username);



        GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;

        finishedMsg.text = "FINISHED";
        finishedMsg.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "FINISHED";

        yield return new WaitForSeconds(3f);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextGameaz();
    }
}
        /*
        void DisplayTime(float timeToDisplay)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        */
    

