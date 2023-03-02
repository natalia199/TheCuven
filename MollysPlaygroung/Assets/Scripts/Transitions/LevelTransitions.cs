using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LevelTransitions : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject btn;

    public List<GameObject> levelInstructions = new List<GameObject>();

    void Start()
    {
        text.text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelNames[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];

        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            levelInstructions[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker].transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            levelInstructions[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker].transform.GetChild(1).gameObject.SetActive(true);
        }

        if (PhotonNetwork.IsMasterClient)
            btn.SetActive(true);
        else
            btn.SetActive(false);
    }

    public void DaButton()
    {
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextSceneButton();
    }
}
