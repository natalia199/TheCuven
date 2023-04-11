using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LevelTransitions : MonoBehaviour
{
    //public TextMeshProUGUI text;
    public GameObject btn;

    public List<Material> bookInstructions = new List<Material>();
    public List<Material> tarotCards = new List<Material>();
    
    public Material prideBookInstruction;
    public Material prideTarotCard;

    //public Vector3 moveHandTo;

    //public GameObject hand;
    //public GameObject RightHand;
    //public GameObject handCards;
    public GameObject tarotCard;
    public GameObject book;

    //public bool moveHandNow = false;


    void Start()
    {
        // SET TAROT CARD AND BOOK MESH HERE

        /*if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker < (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count - 1))
        {
            tarotCard.GetComponent<MeshRenderer>().material = tarotCards[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];
            book.GetComponent<SkinnedMeshRenderer>().material = bookInstructions[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];
        }
        else
        {
            tarotCard.GetComponent<MeshRenderer>().material = tarotCards[tarotCards.Count - 1];
            book.GetComponent<SkinnedMeshRenderer>().material = bookInstructions[tarotCards.Count - 1];
        }
        */

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


        //StartCoroutine("moveHand");

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
