using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LevelTransitions : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Start()
    {
        text.text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelNames[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];
    }
}
