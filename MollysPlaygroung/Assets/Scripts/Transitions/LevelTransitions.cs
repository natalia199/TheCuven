using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LevelTransitions : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject btn;

    void Start()
    {
        text.text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelNames[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];

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
