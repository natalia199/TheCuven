using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LustGameplayManager : MonoBehaviour
{
    public List<GameObject> LifeSlots = new List<GameObject>();

    public List<GameObject> pianoKeys = new List<GameObject>();
    public bool newKey = true;
    public int chosenKey;

    void Start()
    {
    }

    void Update()
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

        pianoKeys[chosenKey].GetComponent<LustPianoKey>().deselectedKey();

        newKey = true;
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().readyForNewKey = true;
    }
}
