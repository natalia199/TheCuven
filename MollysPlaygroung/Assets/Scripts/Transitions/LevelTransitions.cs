using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LevelTransitions : MonoBehaviour
{
    //public TextMeshProUGUI text;
    public GameObject btn;

    /*
    public List<Material> bookInstructions = new List<Material>();
    public List<Material> tarotCards = new List<Material>();
    
    public List<Material> bookInstructionBackup = new List<Material>();
    public List<Material> tarotCardBackup = new List<Material>();
    
    */

    public Material prideBookInstruction;
    public Material prideTarotCard;

    //public Vector3 moveHandTo;

    //public GameObject hand;
    //public GameObject RightHand;
    //public GameObject handCards;
    public GameObject tarotCard;
    public GameObject book;

    public int aliveTracker;

    //public bool moveHandNow = false;


    void Start()
    {
        /*
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().LastLevelPride)
        {
            tarotCard.GetComponent<MeshRenderer>().material = tarotCards[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];
            book.GetComponent<SkinnedMeshRenderer>().material = bookInstructions[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];
        }
        else
        {
            tarotCard.GetComponent<MeshRenderer>().material = prideTarotCard;
            book.GetComponent<SkinnedMeshRenderer>().material = prideBookInstruction;
        }
        */

        aliveTracker = 0;

        for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].stillAlive)
            {
                aliveTracker++;
            }
        }

        if (aliveTracker == 2)
        {
            tarotCard.GetComponent<MeshRenderer>().material = prideTarotCard;
            book.GetComponent<SkinnedMeshRenderer>().material = prideBookInstruction;
        }
        else
        {
            tarotCard.GetComponent<MeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().tarotCards[GameObject.Find("Scene Manager").GetComponent<SceneManage>().chosenLevelIndex];
            book.GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().bookInstructions[GameObject.Find("Scene Manager").GetComponent<SceneManage>().chosenLevelIndex];
        }
    }

    public void DaButton()
    {
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextSceneButton();
    }


    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            btn.SetActive(true);
        }
        else
        {
            btn.SetActive(false);
        }
        
    }
/*
    IEnumerator moveHand()
    {
        yield return new WaitForSeconds(3);

        moveHandNow = true;

        yield return new WaitForSeconds(0.5f);

        RightHand.SetActive(false);
        handCards.SetActive(false);
    }*/
}
