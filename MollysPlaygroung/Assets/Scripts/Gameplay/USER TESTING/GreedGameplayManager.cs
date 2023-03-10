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
    public int startingAmountOfChips;

    public int singlePlayerFinishedState;

    void Start()
    {
        startingAmountOfChips = ChipParent.transform.childCount;


        for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
        {
            chipZones[i].SetActive(true);
            //LifeSlots[i].transform.GetChild(2).gameObject.SetActive(true);
        }

        //goodToGo = false;
        //chipReady = true;
        singlePlayerFinishedState = 0;
        //chipTracker = 0;
    }

    void Update()
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
        {
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                try
                {
                    LifeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];
                    LifeSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chips: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectionTracker + "/" + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().diceRollValue;
                }
                catch (NullReferenceException e)
                {
                }
            }

            chipTracker = chipZones[0].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[1].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[2].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone + chipZones[3].transform.GetChild(1).GetComponent<ChipZoneDetection>().chipsInZone;
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
