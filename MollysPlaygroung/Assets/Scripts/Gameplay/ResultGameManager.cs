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

    void Start()
    {

        PhotonNetwork.Instantiate(gridPlayerTempaltePrefab.name, transform.position, transform.rotation);

        StartCoroutine("HandCrush", 5);
        /*
         player.transform.parent = playerTemplate.transform;
        player.transform.position = Vector3.zero;
        player.transform.localScale *= 6;
        */
        //player.transform.localScale = new Vector3(6f, player.transform.localScale.y, player.transform.localScale.z);
    }

    void Update()
    {

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
