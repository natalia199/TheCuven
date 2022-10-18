using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LevelTransitions : MonoBehaviour
{
    public TextMeshProUGUI text;

    //PhotonView view;

    //public string[] levelTransitions = { "Lust Transition", "Gluttony Transition", "Greed Transition", "Sloth Transition", "Wrath Transition", "Envy Transition", "Pride Transition" };
    public string[] tempLevelTransitions = {"Gluttony Transition", "Greed Transition"};

    void Start()
    {
        //view = GetComponent<PhotonView>();

        StartCoroutine("TransitionTime", 2f);
    }

    IEnumerator TransitionTime(int value)
    {
        Debug.Log("Playing transition");
        text.text = tempLevelTransitions[GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneTracker];

        yield return new WaitForSeconds(value);

        Debug.Log("Transition DONE");

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneInc();
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().sceneChange();
        }
    }
}
