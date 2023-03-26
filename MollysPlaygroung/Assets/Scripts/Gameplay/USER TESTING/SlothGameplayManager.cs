using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class SlothGameplayManager : MonoBehaviour
{
    public List<Transform> TrapSpawnPoints = new List<Transform>();
    public List<GameObject> LifeSlots = new List<GameObject>();

    public List<GameObject> slothResults = new List<GameObject>();

    public GameObject TrapPrefab;
    public GameObject TrapParent;
    public int AmountOfTraps;

    public GameObject LightPrefab;
    public GameObject LightParent;
    bool newLight;

    GameObject oldLight;
    public GameObject currentLight;

    public bool lightReady = true;
    public Vector2 lightPosition;
    public Vector2 trapPosition;
    public bool trapReady = true;

    public int singlePlayerFinishedState;


    void Start()
    {
        singlePlayerFinishedState = 0;
        newLight = true;
    }

    void Update()
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
        {
            
            /*
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                try
                {
                    if (!GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().lifeFullyDone) 
                    {
                        x++;
                    }

                    if (x > 1)
                    {
                        break;
                    }
                    else if (i == (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count - 1) && x < 1)
                    {
                        GameObject.Find("GameManager").GetComponent<TempLevelTimer>().CallGameEnd();
                    }
                }
                catch (NullReferenceException e)
                {
                    break;
                }
            }*/

            // Life display
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                try
                {
                    LifeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];
                    LifeSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Life: " + (int)GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().lifeSource;
                }
                catch (NullReferenceException e)
                {
                    break;
                }
            }

            // INSTANTIATIONS
            try
            {
                if (lightReady && GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theLight != null)
                {
                    addLight(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theLight);
                    lightReady = false;
                }
            }
            catch (NullReferenceException e) { }

            try
            {
                // because of AmountOfTraps != TrapParent.transform.childCount, the last trap doesn't get a rigidbody and is on standby, so if u want X amount of traps on the field input a value of X+1
                if (trapReady && GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theTrap != null && AmountOfTraps != TrapParent.transform.childCount)
                {
                    addTrap(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theTrap);
                    trapReady = false;
                }
            }
            catch (NullReferenceException e) { }
        
        }
    }

    public void addLight(GameObject light)
    {
        Debug.Log("light instant");

        currentLight = light;
        LightInstantiation();
    }

    public void addTrap(GameObject trap)
    {
        Debug.Log("trap instant");

        trap.AddComponent<Rigidbody>();
        TrapInstantiation();
    }

    // BEAR TRAPS
    public void TrapInstantiation()
    {
        StartCoroutine("TrapLifeTime", 2);
    }

    IEnumerator TrapLifeTime(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theTrap = null;
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().instantiateTrapOnce = false;
        trapReady = true;
    }

    // LAMP LIGHT
    public void LightInstantiation()
    {
        StartCoroutine("LightLifeTime", 5);
    }    

    IEnumerator LightLifeTime(float time)
    {
        yield return new WaitForSeconds(time);

        currentLight.GetComponent<LightFlickerSloth>().LightsOut();
        currentLight = null;

        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theLight = null;
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().instantiateLightOnce = false;
        lightReady = true;
    }

    public void RecordSlothResults(GameObject player)
    {
        slothResults.Add(player);
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentLevelsLoser(player.name);
    }
}
