using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SelectionGameManager : MonoBehaviour
{
    //public GameObject btnCanvas;

    public GameObject Target;
    public GameObject spotLight;


    public GameObject charInfo;
    public Material clear;
    public float babyLift;
    public float regPos;

    public float bottomRowSit;
    public float bottomRowStand;
    public float topRowSit;
    public float topRowStand;

    public List<GameObject> characters = new List<GameObject>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       

    }

    public void OnClick_PickCharacter()
    {
        //SceneManager.LoadScene("PlayerRumble");
    }
}
