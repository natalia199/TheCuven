using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerGreedActions : MonoBehaviour
{
    PhotonView view;

    public GameObject [] diceFaces = new GameObject [6];

    int order;

    bool oneTime = false;
    bool gameDone = false;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Greed")
        {
            // Player actions
            if (view.IsMine)
            {
                if (!oneTime)
                {
                    order = 0;

                    for (int i = 0; i < 6; i++)
                    {
                        diceFaces[i] = GameObject.Find("Dice").transform.GetChild(i).gameObject;
                        diceFaces[i].SetActive(false);
                    }

                    diceFaces[0].SetActive(true);

                    oneTime = true;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    diceFaces[order].SetActive(false);
                    order++;
                    if (order > 5)
                    {
                        order = 0;
                    }
                    diceFaces[order].SetActive(true);
                }

                if (GameObject.Find("Gameplay").GetComponent<GreedLevel>().timeLeft < 0 && !gameDone)
                {
                    gameDone = true; 

                    if (PhotonNetwork.IsMasterClient)
                        PhotonNetwork.LoadLevel("Game End");
                }
            }
        }
    }
}
