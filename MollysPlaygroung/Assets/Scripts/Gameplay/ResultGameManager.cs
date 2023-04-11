using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResultGameManager : MonoBehaviour
{
    public GameObject gridGameObject;
    public GameObject gridPlayerTempaltePrefab;
    public Vector3 playerScale;

    public GameObject WheelOfFortune;
    public bool SpinTheWheel = false;
    public bool accessToSpin = false;

    public TextMeshProUGUI wheelText;

    //public GameObject hammer;
    //public bool slowSmash = false;

    public TextMeshProUGUI loserPlayer;
    public TextMeshProUGUI winnerPlayer;

    public float startingCountdown;
    public bool countdownIsRunning = true;

    public GameObject playerHeads;

    public bool oneTimeRound = false;

    void Awake()
    {
        PhotonNetwork.Instantiate(gridPlayerTempaltePrefab.name, transform.position, transform.rotation);
    }

    void Start()
    {
        startingCountdown = 2;
        countdownIsRunning = true;
    }

    void Update()
    {
        if (countdownIsRunning)
        {
           startingCountdown -= Time.deltaTime;

            if (startingCountdown < 0)
            {
                countdownIsRunning = false;
                try { 
                StartCoroutine("HandCrush", 5);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Debug.Log("rude");
                }
            }
        }
    }

    IEnumerator HandCrush(float time)
    {

        loserPlayer.text = "" + GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsLoser[0];

        //yield return new WaitForSeconds(5);

        winnerPlayer.text = "" + GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0];

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().setPlayersLifeStatus(false);


        yield return new WaitForSeconds(10);


        GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsLoser = new List<string>();
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner = new List<string>();
        //SpinTheWheel = true;


        // On to the next scene
        PhotonNetwork.LoadLevel("Card_Scene");     

    }

}
