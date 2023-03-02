using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class TempLevelTimer : MonoBehaviour
{
    public GameObject resultSlots;
    public GameObject bgImg;

    public float timeRemaining;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI waitingMsg;

    // USER DEMO VARIABLES
    public string MyUsername;
    public string CurrentLevel;
    public string MyCompetitorsUsername;
    public double MyCompletionTime;
    public bool GameplayState;
    public string MyCurrentScore;
    public int MyCurrentErrorRate;


    void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        resultSlots.SetActive(false);
    }

    void Update()
    {
        if (timerIsRunning && !GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            timeText.text = "" + Mathf.FloorToInt(timeRemaining);
            timeRemaining += Time.deltaTime;
        }
        else
        {
            timeText.text = "";
        }


        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer && !timerIsRunning)
        {
            bgImg.SetActive(true);
            timeText.text = "";

            if (SceneManager.GetActiveScene().name == "Gluttony")
            {
                if (GetComponent<GluttonyGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }
                else
                {
                    waitingMsg.text = "Waiting for other player to finish...";
                }
            }
            else if (SceneManager.GetActiveScene().name == "Envy")
            {
                if (GetComponent<EnvyGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }

                else
                {
                    waitingMsg.text = "Waiting for other player to finish...";
                }
            }
            else if (SceneManager.GetActiveScene().name == "Wrath")
            {
                if (GetComponent<WrathGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }

                else
                {
                    waitingMsg.text = "Waiting for other player to finish...";
                }
            }
            else if (SceneManager.GetActiveScene().name == "Lust")
            {
                if (GetComponent<LustGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }

                else
                {
                    waitingMsg.text = "Waiting for other player to finish...";
                }
            }
            else if (SceneManager.GetActiveScene().name == "Sloth")
            {
                if (GetComponent<SlothGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }

                else
                {
                    waitingMsg.text = "Waiting for other player to finish...";
                }
            }
            else if (SceneManager.GetActiveScene().name == "Greed")
            {
                if (GetComponent<GreedGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }

                else
                {
                    waitingMsg.text = "Waiting for other player to finish...";
                }
            }
        }
    }

    public void CallGameEnd()
    {
        timerIsRunning = false;

        StartCoroutine("DisplayerFinalResults", 5);
    }

    IEnumerator DisplayerFinalResults(float time)
    {
        bgImg.SetActive(true);

        resultSlots.SetActive(true);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;

        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                try
                {
                    resultSlots.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];

                    if (SceneManager.GetActiveScene().name == "Envy")
                    {
                        int index = -1;

                        for (int j = 0; j < GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().horseResults.Count; j++)
                        {
                            if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().horseName == GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().horseResults[j].name)
                            {
                                index = j + 1;
                                break;
                            }
                        }

                        if (index == -1)
                        {
                            index = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count;

                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                            {
                                MyCurrentErrorRate = 1;
                            }
                        }
                        else
                        {
                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                            {
                                MyCurrentErrorRate = 0;
                            }
                        }

                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                        {
                            MyCurrentScore = "# " + index;
                        }

                        resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Place";

                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "#" + index;
                    }
                    else if (SceneManager.GetActiveScene().name == "Gluttony")
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                        {
                            MyCurrentScore = "" + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies;
                            MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().AmountOfFood - GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies;
                        }

                        resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Munchies";
                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies;
                    }
                    else if (SceneManager.GetActiveScene().name == "Greed")
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                        {
                            MyCurrentScore = "" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                            MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips - GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                        }

                        resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Chipies";
                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                    }
                    else if (SceneManager.GetActiveScene().name == "Sloth")
                    {
                        int index = -1;

                        for (int j = 0; j < GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().slothResults.Count; j++)
                        {
                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().slothResults[j].name)
                            {
                                index = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count + j;
                                break;
                            }
                        }

                        if (index == -1)
                        {
                            index = 1;
                            
                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                            {
                                MyCurrentErrorRate = 0;
                            }
                        }
                        else
                        {
                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                            {
                                MyCurrentErrorRate = 1;
                            }
                        }

                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                        {
                            MyCurrentScore = "# " + index;
                        }

                        resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Place";
                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "# " + index;
                    }
                    else if (SceneManager.GetActiveScene().name == "Lust")
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                        {
                            MyCurrentScore = "" + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys;
                            MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<LustGameplayManager>().maxKeys - GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys;
                        }

                        resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Keys";
                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys;
                    }
                    else if (SceneManager.GetActiveScene().name == "Wrath")
                    {
                        int index = -1;

                        for (int j = 0; j < GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().wrathResults.Count; j++)
                        {
                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().wrathResults[j].name)
                            {
                                index = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count + j;
                                break;
                            }
                        }

                        if (index == -1)
                        {
                            index = 1;

                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                            {
                                MyCurrentErrorRate = 0;
                            }
                        }
                        else
                        {
                            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                            {
                                MyCurrentErrorRate = 1;
                            }
                        }

                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                        {
                            MyCurrentScore = "# " + index;
                        }

                        resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Place";
                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "#" + index;
                    }

                    waitingMsg.text = "On to the next level...";
                }
                catch (NullReferenceException e)
                {
                    break;
                    // error
                }
            }
        }          

        yield return new WaitForSeconds(time);

        setVariables();

        //GameObject.Find("GameManager").GetComponent<RecordUserData>().SendInTheNeededData(MyUsername, CurrentLevel, GameplayState, MyCompetitorsUsername, MyCompletionTime, MyCurrentScore, MyCurrentErrorRate);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextGameaz();
    }


    public void singlePlayerDisplay()
    {
        bgImg.SetActive(true);

        timerIsRunning = false;
        resultSlots.SetActive(true);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;

        try
        {
            resultSlots.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;

            if (SceneManager.GetActiveScene().name == "Envy")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().LifeSlots);

                MyCurrentScore = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
                MyCurrentErrorRate = -1;

                resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
            }
            else if (SceneManager.GetActiveScene().name == "Gluttony")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().LifeSlots);

                MyCurrentScore = "" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().collectedFoodies;
                MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().AmountOfFood - GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().collectedFoodies;

                resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
                //resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().collectedFoodies;
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
            }
            else if (SceneManager.GetActiveScene().name == "Greed")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().LifeSlots);

                for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        MyCurrentScore = "" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                        MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips - GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                        //resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                        resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
                        break;
                    }
                }

                resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
            }
            else if (SceneManager.GetActiveScene().name == "Sloth")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().LifeSlots);

                MyCurrentScore = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
                MyCurrentErrorRate = -1;

                resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
            }
            else if (SceneManager.GetActiveScene().name == "Lust")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<LustGameplayManager>().LifeSlots);

                MyCurrentScore = "" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().hitKeys;
                MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<LustGameplayManager>().maxKeys - GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().hitKeys;

                resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
                //resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().hitKeys;
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
            }
            else if (SceneManager.GetActiveScene().name == "Wrath")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().LifeSlots);

                MyCurrentScore = "" + GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().trackedBoxScore;
                MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().maxAmountOfBoxes - GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().trackedBoxScore;

                resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Boxes";
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().trackedBoxScore;
            }
        }

        catch (NullReferenceException e)
        {
            // error
        }

    }

    void setVariables()
    {
        MyUsername = PhotonNetwork.LocalPlayer.NickName;

        CurrentLevel = GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelNames[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];

        GameplayState = GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer;

        MyCompletionTime = (Mathf.Round(timeRemaining * 100)) / 100.0;

        MyCompetitorsUsername = "nATIS tEST";
        /*
        if (GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().playerNumber == 0)
        {
            MyCompetitorsUsername = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[1];
        }
        else
        {
            MyCompetitorsUsername = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[0];
        }
        */
    }

    void removeSlots(List<GameObject> x)
    {
        for (int i = 0; i < x.Count; i++)
        {
            x[i].SetActive(false);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
