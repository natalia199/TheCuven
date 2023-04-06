using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class EnvyPlayerSpawn : MonoBehaviour
{
    public GameObject playerPrefab;

    void Awake()
    {
        /*
        for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++) {

            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
            {
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer) 
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(transform.GetChild(0).position.x, transform.GetChild(1).position.x), transform.position.y, transform.position.z), Quaternion.identity);
                }
                else
                {
                    GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, singlePlayerSpawn.transform.position, Quaternion.identity);
                    player.GetComponent<PhotonTransformViewClassic>().m_PositionModel.SynchronizeEnabled = false;
                    player.GetComponent<PhotonTransformViewClassic>().m_RotationModel.SynchronizeEnabled = false;
                }
            }
        }
        */

        if (SceneManager.GetActiveScene().name != "Pride")
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(transform.GetChild(0).position.x, transform.GetChild(1).position.x), transform.position.y, transform.position.z), transform.rotation);
        }
        else if (SceneManager.GetActiveScene().name != "Game Ending")
        {
            for(int i= 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
            {
                if(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].stillAlive)
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, GameObject.Find("WinnerSpot").transform.position, transform.rotation);
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(GameObject.Find("LoserSpots").transform.GetChild(0).position.x, GameObject.Find("LoserSpots").transform.GetChild(1).position.x), transform.position.y, Random.Range(GameObject.Find("LoserSpots").transform.GetChild(0).position.z, GameObject.Find("LoserSpots").transform.GetChild(1).position.z)), transform.rotation);
                    }
                    else
                    {
                        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(GameObject.Find("LoserSpots").transform.GetChild(2).position.x, GameObject.Find("LoserSpots").transform.GetChild(3).position.x), transform.position.y, Random.Range(GameObject.Find("LoserSpots").transform.GetChild(2).position.z, GameObject.Find("LoserSpots").transform.GetChild(3).position.z)), transform.rotation);
                    }
                }
            }
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, transform.position, transform.rotation);
        }
    }
}
