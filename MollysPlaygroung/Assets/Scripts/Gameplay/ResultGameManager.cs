using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultGameManager : MonoBehaviour
{
    public GameObject gridGameObject;
    public GameObject gridPlayerTempaltePrefab;
    public Vector3 playerScale;

    //public GameObject hammer;
    //public bool slowSmash = false;

    public TextMeshProUGUI loserPlayer;
    public TextMeshProUGUI winnerPlayer;

    public float startingCountdown;
    public bool countdownIsRunning = true;

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
                StartCoroutine("HandCrush", 5);
            }
        }
    }

    IEnumerator HandCrush(float time)
    {
        loserPlayer.text = "LOSER: " + GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsLoser[0];
        winnerPlayer.text = "WINNER: " + GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0];

        yield return new WaitForSeconds(time);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsLoser = new List<string>();
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner = new List<string>();

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker == GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelNames.Length)
            {

                PhotonNetwork.LoadLevel("Game Ending");

            }
            else
            {
                PhotonNetwork.LoadLevel("Game Level Transition");
            }
        }
    }

}
