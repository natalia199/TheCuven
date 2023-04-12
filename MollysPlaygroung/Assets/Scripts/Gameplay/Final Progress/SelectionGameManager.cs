using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SelectionGameManager : MonoBehaviour
{
    //public GameObject btnCanvas;

    public GameObject usernameScene;
    public GameObject rumbleScene;

    public GameObject selectControl;
    public GameObject confirmControl;

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

    public List<string> charName = new List<string>();
    public List<string> charDesc = new List<string>();

    public List<GameObject> characters = new List<GameObject>();

    void Start()
    {
        usernameScene.SetActive(true);
        rumbleScene.SetActive(false);
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
