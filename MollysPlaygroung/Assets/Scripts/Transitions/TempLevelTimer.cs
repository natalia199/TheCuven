using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempLevelTimer : MonoBehaviour
{
    public float timeRemaining;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
    void Update()
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

                GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState = true;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
