using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;

public class LustGameplayManager : MonoBehaviour
{
    public List<GameObject> LifeSlots = new List<GameObject>();

    public List<GameObject> pianoKeys = new List<GameObject>();
    public bool newKey = true;
    public int chosenKey;

    bool breakLoop = false;
    bool readyForNewKey = false;

    public int maxKeys;
    public int keyAmountTracker;

    public int singlePlayerFinishedState;

    void Start()
    {
        singlePlayerFinishedState = 0;
    }

    void Update()
    {
        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                try
                {
                    LifeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];
                    LifeSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keys: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().hitKeys;
                }
                catch (NullReferenceException e)
                {
                    break;
                    // error
                }
            }
        }
        else
        {
            try
            {
                LifeSlots[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;
                LifeSlots[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keys: " + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().hitKeys;
            }
            catch (NullReferenceException e)
            {
                // error
            }
        }
    }

    public void NewKey(int x)
    {
        newKey = false;
        chosenKey = x;
        StartCoroutine("NewKeyPick", 10);
    } 

    IEnumerator NewKeyPick(float time)
    {
        pianoKeys[chosenKey].GetComponent<LustPianoKey>().selectedKey();

        yield return new WaitForSeconds(time);

        keyAmountTracker++;

        pianoKeys[chosenKey].GetComponent<LustPianoKey>().deselectedKey();

        newKey = true;
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().readyForNewKey = true;
    }

    public void incSinglePlayerState()
    {
        singlePlayerFinishedState++;
    }
}
