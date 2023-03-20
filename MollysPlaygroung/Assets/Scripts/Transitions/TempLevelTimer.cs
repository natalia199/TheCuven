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
        timeRemaining = 0;
        timerIsRunning = true;
        resultSlots.SetActive(false);
    }

    void Update()
    {
        if (timerIsRunning)
        {
            timeText.text = "" + Mathf.FloorToInt(timeRemaining);
            timeRemaining += Time.deltaTime;
        }
        else
        {
            timeText.text = "";
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

        //resultSlots.SetActive(true);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;
        for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
        {
            try
            {
                //resultSlots.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];

                /*
                if (SceneManager.GetActiveScene().name == "Envy")
                {
                    int index = -1;

                    for (int j = 0; j < GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().horseResults.Count; j++)
                    {
                        if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().horseName == GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().horseResults[j].name)
                        {
                            index = j + 1;
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

                    //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Place";

                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "#" + index;
                }
                else if (SceneManager.GetActiveScene().name == "Gluttony")
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        MyCurrentScore = "" + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies;
                        MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().AmountOfFood - GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies;
                    }

                    //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Munchies";
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munched " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies;
                }
                else if (SceneManager.GetActiveScene().name == "Greed")
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        MyCurrentScore = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + "";
                        MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips - GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                    }

                    //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Chipies";
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone +" Chips";
                }
                else if (SceneManager.GetActiveScene().name == "Sloth")
                {
                    int index = -1;

                    for (int j = 0; j < GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().slothResults.Count; j++)
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().slothResults[j].name)
                        {
                            index = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count - j;
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

                    //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Place";
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "# " + index;
                }
                else if (SceneManager.GetActiveScene().name == "Lust")
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        MyCurrentScore = "" + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys;
                        MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<LustGameplayManager>().maxKeys - GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys;
                    }

                    //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Keys";
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text =GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys + " Keys";
                }
                else if (SceneManager.GetActiveScene().name == "Wrath")
                {
                    int index = -1;

                    for (int j = 0; j < GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().wrathResults.Count; j++)
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().wrathResults[j].name)
                        {
                            index = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count - j;
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

                    //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Place";
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "#" + index;
                }
                */

                waitingMsg.text = "On to the next level...";
            }
            catch (NullReferenceException e)
            {
                // error
            }
        }
        

        yield return new WaitForSeconds(time);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextGameaz();
    }


    public void singlePlayerDisplay()
    {
        bgImg.SetActive(true);

        timerIsRunning = false;
        //resultSlots.SetActive(true);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;

        try
        {
            //resultSlots.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;

            /*
            if (SceneManager.GetActiveScene().name == "Envy")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().LifeSlots);

                MyCurrentScore = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
                MyCurrentErrorRate = -1;

                //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = (Mathf.Round(timeRemaining * 100)) / 100.0 + "s";
            }
            else if (SceneManager.GetActiveScene().name == "Gluttony")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().LifeSlots);

                try
                {
                    MyCurrentScore = "" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().collectedFoodies;
                    MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().AmountOfFood - GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().collectedFoodies;
                }
                catch (NullReferenceException e) { }

                //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
                //resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().collectedFoodies;
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0 + "s";
            }
            else if (SceneManager.GetActiveScene().name == "Greed")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().LifeSlots);

                try
                {
                    for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                        {
                            MyCurrentScore = "" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                            MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips - GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                            //resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
                            break;
                        }
                    }
                }
                catch (NullReferenceException e) { }

                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0 + "s";
                //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
            }
            else if (SceneManager.GetActiveScene().name == "Sloth")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().LifeSlots);

                MyCurrentScore = "" + (Mathf.Round(timeRemaining * 100)) / 100.0;
                MyCurrentErrorRate = -1;

                //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0 + "s";
            }
            else if (SceneManager.GetActiveScene().name == "Lust")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<LustGameplayManager>().LifeSlots);

                try
                {
                    MyCurrentScore = "" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().hitKeys;
                    MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<LustGameplayManager>().maxKeys - GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().hitKeys;
                }
                catch (NullReferenceException e){ }

                //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Time";
                //resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().hitKeys;
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(timeRemaining * 100)) / 100.0 + "s";
            }
            else if (SceneManager.GetActiveScene().name == "Wrath")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().LifeSlots);

                MyCurrentScore = "" + GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().trackedBoxScore;
                MyCurrentErrorRate = GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().maxAmountOfBoxes - GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().trackedBoxScore;

                //resultSlots.transform.GetChild(resultSlots.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "Boxes";
                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().trackedBoxScore + " Boxes";
            }*/
        }

        catch (NullReferenceException e)
        {
            // error
        }


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
