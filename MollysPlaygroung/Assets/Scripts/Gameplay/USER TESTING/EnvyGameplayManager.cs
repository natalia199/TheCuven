using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnvyGameplayManager : MonoBehaviour
{
    public List<GameObject> racingHorseys = new List<GameObject>();
    public List<GameObject> horseResults = new List<GameObject>();

    public List<GameObject> LifeSlots = new List<GameObject>();

    public int singlePlayerFinishedState;

    public GameObject scoreBoard;

    // Start is called before the first frame update
    void Start()
    {
        singlePlayerFinishedState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            try
            {
                LifeSlots[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;
                LifeSlots[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = horseResults.Count + "/" + racingHorseys.Count + " horses";
                LifeSlots[0].transform.GetChild(2).gameObject.SetActive(false);

                LifeSlots[1].SetActive(false);
                LifeSlots[2].SetActive(false);
                LifeSlots[3].SetActive(false);

                scoreBoard.SetActive(false);
            }
            catch (NullReferenceException e)
            {
                // error
            }
        }
        else
        {
            scoreBoard.SetActive(true);

            // Life display
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                try
                {
                    LifeSlots[i].SetActive(true);

                    LifeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];

                    if (GameObject.Find(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().horseName).GetComponent<EnvyHorse>().finished)
                    {
                        LifeSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Finished";
                    }
                }
                catch (NullReferenceException e)
                {
                    break;
                }
            }
        }
    }

    public void RecordHorseResult(GameObject horse)
    {
        horseResults.Add(horse);
    }

    public void endingGameDelay()
    {
        StartCoroutine("HoldIt", 5);
    }

    IEnumerator HoldIt(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState = true;
    }

    public void MoveHorseForward(string name)
    {
        GameObject.Find(name).GetComponent<EnvyHorse>().MoveYourHorse();
    }

    public void StopHorseForward(string name)
    {
        GameObject.Find(name).GetComponent<EnvyHorse>().StopYourHorse();
    }
    public void squirtWater(string name)
    {
        GameObject.Find(name).transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = true;
    }

    public void desquirtWater(string name)
    {
        GameObject.Find(name).transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = false;
    }

    public void MoveHorseForwardSP(GameObject gun)
    {
        GameObject.Find(gun.transform.GetChild(0).GetComponent<EnvySquirter>().correlatingHorse).GetComponent<EnvyHorse>().MoveYourHorse();
    }

    public void StopHorseForwardSP(GameObject gun)
    {
        GameObject.Find(gun.transform.GetChild(0).GetComponent<EnvySquirter>().correlatingHorse).GetComponent<EnvyHorse>().StopYourHorse();
    }
    public void squirtWateSP(GameObject gun)
    {
        gun.transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = true;
    }

    public void desquirtWaterSP(GameObject gun)
    {
        gun.transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = false;
    }

    public void incSinglePlayerState()
    {
        singlePlayerFinishedState++;
    }
}
