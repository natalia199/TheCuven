using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GreedGameplayManager : MonoBehaviour
{
    public List<GameObject> chipZones = new List<GameObject>();

    public GameObject ChipPrefab;
    public GameObject ChipParent;

    public int AmountOfChips;
    public int rolledValue;
    public bool goodToGo = false;

    public Vector2 chipPosition;
    public bool chipReady = true;

    public int chipTracker;
    public int startingAmountOfChips;

    void Start()
    {
        startingAmountOfChips = ChipParent.transform.childCount;

        for (int i = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i < chipZones.Count; i++)
        {
            chipZones[i].transform.GetChild(1).GetComponent<ChipZoneDetection>().activatedForPlayer = false;
            chipZones[i].transform.GetChild(2).gameObject.SetActive(false);
            chipZones[i].transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        chipTracker = chipZones[0].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[1].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[2].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[3].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone+ chipZones[4].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[5].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[6].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[7].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
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
