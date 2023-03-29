using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnvyGameplayManager : MonoBehaviour
{
    public List<string> EnvyResults = new List<string>();

    public GameObject scoreBoard;

    public void RecordHorseResult(GameObject horse)
    {
        EnvyResults.Add(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[horse.GetComponent<EnvyHorse>().id].username);
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsWinner(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[horse.GetComponent<EnvyHorse>().id].username);
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
