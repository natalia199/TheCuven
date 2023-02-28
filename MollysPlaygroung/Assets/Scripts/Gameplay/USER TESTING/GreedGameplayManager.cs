using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GreedGameplayManager : MonoBehaviour
{
    //public List<Transform> ChipSpawnPoints = new List<Transform>();
    public List<GameObject> LifeSlots = new List<GameObject>();
    public List<GameObject> chipZones = new List<GameObject>();

    public GameObject ChipPrefab;
    public GameObject ChipParent;

    public int AmountOfChips;
    public int rolledValue;
    public bool goodToGo = false;

    public Vector2 chipPosition;
    public bool chipReady = true;

    public int chipTracker;

    void Start()
    {
        chipZones[0].SetActive(false);
        chipZones[1].SetActive(false);
        chipZones[2].SetActive(false);
        chipZones[3].SetActive(false);

        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            chipZones[0].SetActive(true);
            chipZones[1].SetActive(false);
            chipZones[2].SetActive(false);
            chipZones[3].SetActive(false);
        }
        else
        {
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                chipZones[i].SetActive(true);
            }
        }
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
                    LifeSlots[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chips: " + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent <PlayerUserTest>().collectionTracker + "/" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue;
                }
                catch (NullReferenceException e)
                {
                    // error
                }

                /*
                // Instantiation
                if (AmountOfChips > ChipParent.transform.childCount)
                {
                    addChip(null);
                }
                */
            }
            else
            {
                // Life display
                for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
                {
                    try
                    {
                        LifeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];
                        LifeSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chips: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectionTracker + "/" + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().diceRollValue;
                    }
                    catch (NullReferenceException e)
                    {
                        break;
                    }
                }

                /*
                try
                {
                    // because of AmountOfTraps != TrapParent.transform.childCount, the last trap doesn't get a rigidbody and is on standby, so if u want X amount of traps on the field input a value of X+1
                    if (chipReady && GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theChip != null && AmountOfChips != ChipParent.transform.childCount)
                    {
                        addChip(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theChip);
                        chipReady = false;
                    }
                }
                catch (NullReferenceException e) { }
                */
            }
        }
    } 
    
    public void incChipCount()
    {
        chipTracker++;
    }
/*
    public void addChip(GameObject chip)
    {
        Debug.Log("chip instant");

        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            chip.AddComponent<Rigidbody>();
            ChipInstantiation();
        }
        else
        {
            StartCoroutine("ChipLifeTimeSP", UnityEngine.Random.Range(0.3f, 4));
        }
    }

    public void ChipInstantiation()
    {
        StartCoroutine("ChipLifeTime", 0.3f);
    }

    IEnumerator ChipLifeTime(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theChip = null;
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().instantiateChipOnce = false;
        chipReady = true;
    }

    IEnumerator ChipLifeTimeSP(float time)
    {
        float xPos = UnityEngine.Random.Range(ChipSpawnPoints[0].position.x, ChipSpawnPoints[2].position.x);
        float zPos = UnityEngine.Random.Range(ChipSpawnPoints[0].position.z, ChipSpawnPoints[1].position.z);
        Vector3 posy = new Vector3(xPos, 12, zPos);

        GameObject chip = Instantiate(ChipPrefab, posy, Quaternion.identity, ChipParent.transform);

        yield return new WaitForSeconds(time);

        chip.AddComponent<Rigidbody>();
    }

   

    // OLD

    public void addChip(GameObject chip)
    {
        chip.AddComponent<Rigidbody>();
        ChipInstantiation();
    }

    public void ChipInstantiation()
    {
        StartCoroutine("HoldIt", 0.3f);
    }

    IEnumerator HoldIt(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theChip = null;
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().instantiateChipOnce = false;
        chipReady = true;
    }

    
    public void ChipInstantiation()
    {
        StartCoroutine("HoldIt", Random.Range(0, 3));
    }

    void UpdateChipAmount()
    {
        int x = AmountOfChips - ChipParent.transform.childCount;
        for (int i = 0; i < x; i++)
        {
            ChipInstantiation();
        }
    }


    IEnumerator HoldIt(float time)
    {
        float xPos = Random.Range(ChipSpawnPoints[0].position.x, ChipSpawnPoints[2].position.x);
        float zPos = Random.Range(ChipSpawnPoints[0].position.z, ChipSpawnPoints[1].position.z);

        GameObject chip = Instantiate(ChipPrefab, new Vector3(xPos, 12f, zPos), Quaternion.identity, ChipParent.transform);

        chip.transform.rotation = new Quaternion(Random.Range(15, -15), 0, Random.Range(15, -15), chip.transform.rotation.w);

        yield return new WaitForSeconds(time);

        chip.AddComponent<Rigidbody>();
    }
    */
}
