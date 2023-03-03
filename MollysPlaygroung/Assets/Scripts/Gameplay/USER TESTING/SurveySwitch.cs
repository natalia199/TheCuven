using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SurveySwitch : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject btn;

    void Start()
    {
        text.text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelNames[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker - 1];

        if (PhotonNetwork.IsMasterClient)
            btn.SetActive(true);
        else
            btn.SetActive(false);
    }

    public void DaSurveyButton()
    {
        PhotonNetwork.LoadLevel("Game Level Transition");
    }
}
