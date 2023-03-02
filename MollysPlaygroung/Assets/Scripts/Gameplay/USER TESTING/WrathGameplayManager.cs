using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;

public class WrathGameplayManager : MonoBehaviour
{
    public int directionIndex;
    public bool returnHome;

    public GameObject lava;
    public float rotationsPerMinute;

    public GameObject wrathPlate;

    public bool plateState = false;

    // player score
    public int trackedBoxScore;
    // max boxes in scene
    public int AmountOfBoxes;
    // boxes on screen
    public int TrackBoxes;
    // overall max boxes to collect
    public int maxAmountOfBoxes;
    // box counter
    public int boxesInstantiated;

    public GameObject boxParent;
    public List<Transform> BoxpawnPoints = new List<Transform>();
    public GameObject boxPrefab;

    public GameObject singlePlayerPlatform;
    public GameObject multiPlayerPlatform;
    public GameObject boxScoreTxt;
    public bool noMoreBoxesNeeded = false;

    public List<GameObject> wrathResults = new List<GameObject>();
    public List<GameObject> LifeSlots = new List<GameObject>();

    public int singlePlayerFinishedState;

    void Start()
    {
        plateState = wrathPlate.GetComponent<PlatformShakeWrath>().newTilt;

        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            singlePlayerPlatform.SetActive(true);
            multiPlayerPlatform.SetActive(false);
        }
        else
        {
            boxScoreTxt.SetActive(false);
            singlePlayerPlatform.SetActive(false);
            multiPlayerPlatform.SetActive(true);
        }

        singlePlayerFinishedState = 0;
        trackedBoxScore = 0;
    }

    void Update()
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
        {
            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
            {
                try
                {
                    LifeSlots[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;
                    LifeSlots[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = trackedBoxScore + "/" + maxAmountOfBoxes;
                }
                catch (NullReferenceException e)
                {
                    // error
                }
            }
        }
        else
        {
            // Life display
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                try
                {
                    LifeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];

                    if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().fellOffPlatform)
                    {
                        LifeSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Finished";
                    }
                }
                catch (NullReferenceException e)
                {
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            if (AmountOfBoxes > TrackBoxes && !noMoreBoxesNeeded) {
                // Instantiation
                for (int i = 0; i < (AmountOfBoxes - TrackBoxes); i++)
                {
                    float xPos = UnityEngine.Random.Range(BoxpawnPoints[0].position.x, BoxpawnPoints[2].position.x);
                    float zPos = UnityEngine.Random.Range(BoxpawnPoints[0].position.z, BoxpawnPoints[1].position.z);
                    Vector3 posy = new Vector3(xPos, 5, zPos);

                    Instantiate(boxPrefab, posy, Quaternion.identity, boxParent.transform);
                    TrackBoxes++;
                }
            }
        }

        lava.transform.Rotate(0, 6 * rotationsPerMinute * Time.deltaTime, 0);

        plateState = wrathPlate.GetComponent<PlatformShakeWrath>().newTilt;
    }

    public void shakePlateVariables(int dir)
    {
        wrathPlate.GetComponent<PlatformShakeWrath>().newTilt = false;
        plateState = false;
        wrathPlate.GetComponent<PlatformShakeWrath>().setVars(dir);
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().plateMoving = false;
    }

    public void RecordWrathResults(GameObject player)
    {
        wrathResults.Add(player);
    }

    public void incSinglePlayerState()
    {
        singlePlayerFinishedState++;
    }

}
