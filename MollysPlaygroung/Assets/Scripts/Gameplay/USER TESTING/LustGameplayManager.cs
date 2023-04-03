using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;

public class LustGameplayManager : MonoBehaviour
{
    public List<GameObject> pianoKeys = new List<GameObject>();

    public bool newKey = true;
    public int chosenKey;

    bool breakLoop = false;

    public int maxKeys;
    public int keyAmountTracker;

    public bool readyForNewKey = true;

    public bool keyChosen = false;

    public bool timerActivated = false;

    void Start()
    {
        newKey = true;
        readyForNewKey = true;
    }

    void Update()
    {

    }

    public void NewKey(int x)
    {
        newKey = false;
        chosenKey = x;

        pianoKeys[chosenKey].GetComponent<LustPianoKey>().selectedKey();

        timerActivated = true;
        //StartCoroutine("NewKeyPick", 10);
    } 

    public void holdKeyPick()
    {
        StartCoroutine("holdForABit");
    }

    IEnumerator holdForABit()
    {
        keyAmountTracker++;

        pianoKeys[chosenKey].GetComponent<LustPianoKey>().deselectedKey();

        yield return new WaitForSeconds(2);

        newKey = true;
        readyForNewKey = true;
        keyChosen = false;
    }
   
}
