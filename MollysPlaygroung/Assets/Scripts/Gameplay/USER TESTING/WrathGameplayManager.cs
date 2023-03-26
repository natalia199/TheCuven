using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;

public class WrathGameplayManager : MonoBehaviour
{
    // lava variables
    public int directionIndex;
    public bool returnHome;

    public GameObject lava;
    public float rotationsPerMinute;

    public GameObject wrathPlate;

    public bool plateState = false;

    // player variables
    //public GameObject multiPlayerPlatform;
    public List<GameObject> wrathResults = new List<GameObject>();

    void Start()
    {
        plateState = wrathPlate.GetComponent<PlatformShakeWrath>().newTilt;
    }

    void Update()
    {
        /*
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
        */
    }

    void FixedUpdate()
    {
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
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(player.name);
    }
    
}
