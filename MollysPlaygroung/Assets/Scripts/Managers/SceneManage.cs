using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SceneManage : MonoBehaviour
{
    public int sceneTracker;

    //public string[] levelNames = {"Lust", "Gluttony", "Greed", "Sloth", "Wrath", "Envy", "Pride"};
    public string[] tempLevelNames = {"Gluttony", "Greed"};

    void Start()
    {
        DontDestroyOnLoad(this);

        sceneTracker = 0;
    }

    public void sceneChange()
    {
        //PhotonNetwork.LoadLevel(levelNames[sceneTracker]);
        PhotonNetwork.LoadLevel(tempLevelNames[sceneTracker - 1]);
    }

    public void sceneInc()
    {
        sceneTracker++;
        Debug.Log("Scene " + sceneTracker);
    }
}
