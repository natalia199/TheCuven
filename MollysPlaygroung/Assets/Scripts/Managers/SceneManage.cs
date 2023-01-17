using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SceneManage : MonoBehaviour
{
    public int sceneTracker;

    //public string[] levelNames = {"Lust", "Gluttony", "Greed", "Sloth", "Wrath", "Envy", "Pride"};
    public string[] tempLevelNames = { "Gluttony", "Greed", "Sloth" };

    public List<string> allPlayersInGame = new List<string>();

    void Start()
    {
        DontDestroyOnLoad(this);

        sceneTracker = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            allPlayersInGame.Add(player.NickName);
        }
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
