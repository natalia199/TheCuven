using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempLevelTimer : MonoBehaviour
{
    public GameObject resultSlots;

    public float timeRemaining;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;

    void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        resultSlots.SetActive(false);
    }
    void Update()
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeText.text = "" + Mathf.FloorToInt(timeRemaining);
                    timeRemaining -= Time.deltaTime;
                    //DisplayTime(timeRemaining);
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeText.text = "0";
                    timeRemaining = 0;
                    timerIsRunning = false;

                    //GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState = true;
                    StartCoroutine("DisplayerFinalResults", 10);
                }
            }
        }
    }

    public void CallGameEnd()
    {
        StartCoroutine("DisplayerFinalResults", 10);
    }

    IEnumerator DisplayerFinalResults(float time)
    {
        resultSlots.SetActive(true);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true; 

        for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
        {
            try
            {
                resultSlots.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];

                if (SceneManager.GetActiveScene().name == "Envy")
                {
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                else if (SceneManager.GetActiveScene().name == "Gluttony")
                {
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munchies: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies;
                }
                else if(SceneManager.GetActiveScene().name == "Greed")
                {
                    //resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chipies : " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().;
                }
                else if(SceneManager.GetActiveScene().name == "Sloth")
                {
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Life: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().lifeSource;
                }
                else if (SceneManager.GetActiveScene().name == "Lust")
                {
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keys: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys;
                }
                else if (SceneManager.GetActiveScene().name == "Wrath")
                {
                    resultSlots.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Boxes: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().boxScore;
                }
            }
            catch (NullReferenceException e)
            {
                break;
                // error
            }
        }

        yield return new WaitForSeconds(time);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
