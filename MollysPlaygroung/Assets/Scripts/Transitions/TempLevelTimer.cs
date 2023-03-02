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

    public float timeRemaining;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI waitingMsg;


    void Start()
    {
        // Starts the timer automatically
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


        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer && !timerIsRunning)
        {
            /*
            if (SceneManager.GetActiveScene().name == "Greed")
            {
                if (GetComponent<GreedGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }
            }*/
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

            /*
            else if (SceneManager.GetActiveScene().name == "Sloth")
            {
                if (GetComponent<SlothGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }
            }
            else if (SceneManager.GetActiveScene().name == "Wrath")
            {
                if (GetComponent<WrathGameplayManager>().singlePlayerFinishedState == GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count)
                {
                    waitingMsg.text = "Entering Multiplayer!";
                }
            }*/
        }
    }

    public void CallGameEnd()
    {
        timerIsRunning = false;

        StartCoroutine("DisplayerFinalResults", 5);
    }

    IEnumerator DisplayerFinalResults(float time)
    {
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
                        }

                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "#" + index;
                    }
                    else if (SceneManager.GetActiveScene().name == "Gluttony")
                    {
                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munchies: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies;
                    }
                    else if (SceneManager.GetActiveScene().name == "Greed")
                    {
                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chipies : " + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().zoneCollider.Length / 2;
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
                        }

                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Life: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().lifeSource;
                    }
                    else if (SceneManager.GetActiveScene().name == "Lust")
                    {
                        resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keys: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys;
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
                        }

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

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextGameaz();
    }


    public void singlePlayerDisplay()
    {
        timerIsRunning = false;
        resultSlots.SetActive(true);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;

        try
        {
            resultSlots.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;

            if (SceneManager.GetActiveScene().name == "Envy")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().LifeSlots);

                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            else if (SceneManager.GetActiveScene().name == "Gluttony")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().LifeSlots);

                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munchies: " + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().collectedFoodies;
            }
            else if (SceneManager.GetActiveScene().name == "Greed")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().LifeSlots);

                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chipies : " + GameObject.Find("Bucket" + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().playerNumber).transform.GetChild(1).GetComponent<ChipZoneDetection>().zoneCollider.Length / 2;
            }
            else if (SceneManager.GetActiveScene().name == "Sloth")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().LifeSlots);

                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            else if (SceneManager.GetActiveScene().name == "Lust")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<LustGameplayManager>().LifeSlots);

                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keys: " + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().hitKeys;
            }
            else if (SceneManager.GetActiveScene().name == "Wrath")
            {
                removeSlots(GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().LifeSlots);

                resultSlots.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Boxes: " + GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().trackedBoxScore;
            }
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
