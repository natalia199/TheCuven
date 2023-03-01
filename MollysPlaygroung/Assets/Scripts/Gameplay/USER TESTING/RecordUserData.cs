using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RecordUserData : MonoBehaviour
{
    // Google Forms URL for data
    string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdtfmwbiVuWPjWMEMY5GgRmbjZQhr5zlCvi3Zf1Bzyoemt8cg/formResponse";

    float loopTimer = 0.0f;

    void Awake()
    {
        newScene();                                                 // Reset variables for new trial order
    }

    public static void newScene()
    {

    }

    // Convert data into String vairables
    public void Send(string username, string level, bool singleP, string competitor, int timeCompletion, int score_data, int errorRate_data)
    {
        string singlePlayer = "" + singleP;
        string time = "" + timeCompletion;
        string score = "" + score_data;
        string errorRate = "" + errorRate_data;

        StartCoroutine(Post(username, level, singlePlayer, competitor, time, score, errorRate));
    }

    // Send data to the Google Form
    IEnumerator Post(string username, string level, string singlePlayer, string competitor, string timeCompletion, string score, string errorRate)
    {
        WWWForm form = new WWWForm();

        form.AddField("entry.829246064", username);                               // ID
        form.AddField("entry.2111353321", level);                       // order
        form.AddField("entry.84197714", singlePlayer);                        // FOV horizontal size
        form.AddField("entry.45014326", competitor);                       // FOV vertical size
        form.AddField("entry.639620268", timeCompletion);                      // Speed
        form.AddField("entry.443305856", score);                       // view
        form.AddField("entry.1255032601", errorRate);                      // Trial

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        Debug.Log("submitted");
    }

    public void SendInTheNeededData(string username, string level, bool singleP, string competitor, int timeCompletion, int score, int errorRate)
    {
        //Send(userID.ID, GameObject.Find("DontDestroy").GetComponent<setUp_N>().order + 1, currentHorFOV, currentVerFOV, targetSpeed, view, trialID, setUp_N.chosenCondition[trials_N.trialTracker - 1].conditionID, setNumTargets, numTargets, time, errorRate, inFOVCount, timeInFOV, ObjectController.targetEnterCount, timeonTarget, targetDistance);
        Send(username, level, singleP, competitor, timeCompletion, score, errorRate);
    }
}
