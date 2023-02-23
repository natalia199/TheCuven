using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedGameplayManager : MonoBehaviour
{
    public List<Transform> ChipSpawnPoints = new List<Transform>();

    public GameObject ChipPrefab;
    public GameObject ChipParent;

    public int AmountOfChips;
    public int rolledValue;
    public bool goodToGo = false;

    public Vector2 chipPosition;
    public bool chipReady = true;

    void Start()
    {

    }


    void Update()
    {
        try
        {
            // because of AmountOfTraps != TrapParent.transform.childCount, the last trap doesn't get a rigidbody and is on standby, so if u want X amount of traps on the field input a value of X+1
            if (chipReady && GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theChip != null && AmountOfChips != ChipParent.transform.childCount)
            {
                addChip(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theChip);
                chipReady = false;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

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

    /*
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
