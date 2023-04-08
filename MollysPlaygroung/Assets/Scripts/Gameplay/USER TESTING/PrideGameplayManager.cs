using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrideGameplayManager : MonoBehaviour
{
    public List<GameObject> CupSets = new List<GameObject>();

    public List<GameObject> prideResults = new List<GameObject>();

    public List<string> listOfPridePlayers = new List<string>();                    // order of players so [0] == poisoner and [1] == drinker
    
    public string poisonedCup;                                                  // chosen cup to be poisoned
    public string drinkingCup;                                                  // chosen cup to be drank

    public bool playerActionChoice = false;
    public bool resultCheck = false;
    public bool gameOver = false;
    public bool resetCups = false;
    public bool discussionTime = false;

    public int playerTurnTracker = 0;

    public TextMeshProUGUI playerTurnDisplay;
    public TextMeshProUGUI chosenCupText;

    
    public List<float> timeStamps = new List<float>();
    /*public List<string> timeStampNames = new List<string>();

    public int currentTimeStamp;

    public bool turnTracker;
    */
    public TextMeshProUGUI timeStampText;
    public bool timerSetAfterLoad = false;
    public int timeStampTrack = 0;
    /*
    public GameObject resultCanvas;

    public bool processSwitch = false;
    public bool discussionPeriod = false;
    */

    void Start()
    {
        //timerIsRunning = true;

        for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive)
            {
                if (listOfPridePlayers.Count == 0)
                {
                    listOfPridePlayers.Add(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username);
                }
                else
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].deathTracker == GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x -1].deathTracker)
                    {
                        // chance selection activated
                        listOfPridePlayers.Add(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username);
                    }
                    else if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].deathTracker < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x - 1].deathTracker)
                    {
                        // add player after first player
                        listOfPridePlayers.Add(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username);
                    }
                    else if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].deathTracker > GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x - 1].deathTracker)
                    {
                        // add player before first player
                        listOfPridePlayers.Insert(0, GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username);
                    }
                }
            }
        }

        playerActionChoice = true;
        playerTurnTracker = 0;
        timeStampTrack = 0;
        playerTurnDisplay.text = listOfPridePlayers[0] + " turn";
    }

    void Update()
    {
        /*
        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().countdownLevelCheck && )
        {
            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().prideTimer = timeStamps[timeStampTrack];
        }
        */
    }

    public void setPoisonedCup(string pname, string cup)
    {
        poisonedCup = cup;

        StartCoroutine("likklePause", 3f);
    }

    public void setDrinkingCup(string pname, string cup)
    {
        drinkingCup = cup;

        StartCoroutine("likklePause", 3f);
    }

    IEnumerator likklePause(int value)
    {
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().prideTimerRunning = false;
        timeStampText.text = "";

        yield return new WaitForSeconds(value);

        chosenCupText.text = "";
        playerActionChoice = false;

        if (playerTurnTracker == 0)
        {
            // GO TO DISCUSSION
            timeStampTrack = 1;
            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().prideTimerRunning = true;
            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().prideTimer = timeStamps[timeStampTrack];
            playerTurnDisplay.text = "Discussion Time";
            discussionTime = true;
        }
        else if(playerTurnTracker == 1)
        {
            playerTurnDisplay.text = "";
            //resultCheck = true;
            playerActionChoice = false;

            StartCoroutine("likkleResultDisplay", 5);
        }
    }

    public void timesUpWrapItUp(bool state)
    {
        // player choice time run out
        if (state)
        {
            if (playerTurnTracker == 0)
            {
                setPoisonedCup(listOfPridePlayers[0], "1");
            }
            else if (playerTurnTracker == 1)
            {
                setDrinkingCup(listOfPridePlayers[1], "1");
            }
        }
        // dicussiion time run out
        else
        {
            discussionTime = false;
            playerTurnTracker = 1;
            timeStampTrack = 2;
            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().prideTimerRunning = true;
            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().prideTimer = timeStamps[timeStampTrack];
            playerTurnDisplay.text = listOfPridePlayers[playerTurnTracker] + " turn";
            playerActionChoice = true;
        }
    }
    

    IEnumerator likkleResultDisplay(int value)
    {
        resetCups = true;

        if (poisonedCup == drinkingCup)
        {
            playerTurnDisplay.text = listOfPridePlayers[1] + " poisoned";
            GameObject.Find(listOfPridePlayers[1]).GetComponent<PlayerUserTest>().gotPoisoned = true;
            GameObject.Find(poisonedCup).transform.GetChild(0).gameObject.SetActive(true);
            //GameObject.Find("RoomLights").gameObject.SetActive(false);
            chosenCupText.text = "";
            gameOver = true;
        }
        else
        {
            playerTurnDisplay.text = listOfPridePlayers[1] + " survived";
            chosenCupText.text = "Role Switch";

            GameObject.Find(poisonedCup).transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find(drinkingCup).transform.GetChild(0).gameObject.SetActive(true);
            //GameObject.Find("RoomLights").gameObject.SetActive(false);

            string player = listOfPridePlayers[0];
            listOfPridePlayers.RemoveAt(0);
            listOfPridePlayers.Add(player);
        }

        yield return new WaitForSeconds(value);

        if (gameOver)
        {
            RecordPrideResults(GameObject.Find(listOfPridePlayers[1]));
        }
        else
        {
            GameObject.Find(poisonedCup).transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find(drinkingCup).transform.GetChild(0).gameObject.SetActive(false);
            //GameObject.Find("RoomLights").gameObject.SetActive(true);

            poisonedCup = "";
            drinkingCup = "";
            playerTurnTracker = 0;

            playerActionChoice = true;
            playerTurnDisplay.text = listOfPridePlayers[0] + " turn";
            chosenCupText.text = "";
            timeStampTrack = 0;
            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().prideTimerRunning = true;
            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().prideTimer = timeStamps[timeStampTrack];
        }

        resetCups = false;
    }

    public void RecordPrideResults(GameObject player)
    {
        prideResults.Add(player);
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(player.name);
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (startingCountdown > 1)
            {
                timeStampText.text = "" + Mathf.FloorToInt(startingCountdown);
            }
            else
            {
                timeStampText.text = "Time's Up";
            }

            startingCountdown -= Time.deltaTime;

            if (startingCountdown < 0)
            {
                timerIsRunning = false;
            }
        }
        else
        {
            timeStampText.text = timeStampNames[currentTimeStamp];
            startingCountdown = timeStamps[currentTimeStamp];
        }
    }

    


    IEnumerator Results(int value)
    {
        // check if chosen cup vs posined cup is the SAME

        if (poisonedCup.name == drinkingCup.name)
        {

        }
        else
        {

        }

        yield return new WaitForSeconds(value);

        turnTracker = !turnTracker;

    }
    */
}
