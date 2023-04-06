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

    public Vector3 moveHandTo;

    public GameObject hand;
    public GameObject RightHand;
    public GameObject handCards;
    public GameObject tarotCard;
    public GameObject book;

    public bool moveHandNow = false;


    void Start()
    {
        // SET TAROT CARD AND BOOK MESH HERE

        tarotCard.GetComponent<MeshRenderer>().material = tarotCards[GameObject.Find("Scene Manager").GetComponent<SceneManage>().chosenLevelIndex];
        book.GetComponent<SkinnedMeshRenderer>().material = bookInstructions[GameObject.Find("Scene Manager").GetComponent<SceneManage>().chosenLevelIndex];

        StartCoroutine("moveHand");

    }

    public void DaButton()
    {
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextSceneButton();
    }

    void Update()
    {
        //GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = false;

        if (moveHandNow)
        {
            hand.transform.position = Vector3.Lerp(hand.transform.position, moveHandTo, Time.deltaTime * 3f);
        }


        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            btn.SetActive(true);
        }
        else
        {
            btn.SetActive(false);
        }

    }

    IEnumerator moveHand()
    {
        yield return new WaitForSeconds(3);

        moveHandNow = true;

        yield return new WaitForSeconds(0.5f);

        RightHand.SetActive(false);
        handCards.SetActive(false);
    }
}
