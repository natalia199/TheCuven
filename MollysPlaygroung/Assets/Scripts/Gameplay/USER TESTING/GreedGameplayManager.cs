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

    public GameObject diceControls;

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
}
