using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlothGameplayManager : MonoBehaviour
{
    public List<Transform> TrapSpawnPoints = new List<Transform>();
    public List<GameObject> LifeSlots = new List<GameObject>();

    public GameObject TrapPrefab;
    public GameObject TrapParent;
    public int AmountOfTraps;

    public GameObject LightPrefab;
    public GameObject LightParent;
    bool newLight;

    GameObject oldLight;
    public GameObject currentLight;

    public bool lightReady = true;
    public bool trapReady = true;

    public Vector2 lightPosition;
    public Vector2 trapPosition;

    void Start()
    {
        newLight = true;
    }

    void Update()
    {
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
                // error
            }
        }

        try
        {
            if (lightReady && GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theLight != null)
            {
                addLight(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theLight);
                lightReady = false;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }

        try
        {
            // because of AmountOfTraps != TrapParent.transform.childCount, the last trap doesn't get a rigidbody and is on standby, so if u want X amount of traps on the field input a value of X+1
            if (trapReady && GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theTrap != null && AmountOfTraps != TrapParent.transform.childCount)
            {
                addTrap(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theTrap);
                trapReady = false;
            }
        }
        catch (NullReferenceException e)
        {
            // error
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
        StartCoroutine("HoldIt", 2);
    }

    IEnumerator HoldIt(float time)
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
}
