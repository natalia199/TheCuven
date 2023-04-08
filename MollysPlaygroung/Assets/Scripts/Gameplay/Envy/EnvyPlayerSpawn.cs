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
        if (SceneManager.GetActiveScene().name == "Pride")
        {
            PhotonNetwork.Instantiate(playerPrefab.name, transform.position, new Quaternion(0, 180, 0, transform.rotation.w));
        }
        else if (SceneManager.GetActiveScene().name == "Game Ending")
        {
            if (PhotonNetwork.LocalPlayer.NickName == GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0])
            {
                PhotonNetwork.Instantiate(playerPrefab.name, GameObject.Find("WinnerSpot").transform.position, new Quaternion(0, 180, 0, transform.rotation.w));
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(GameObject.Find("LoserSpots").transform.GetChild(0).position.x, GameObject.Find("LoserSpots").transform.GetChild(1).position.x), transform.position.y, Random.Range(GameObject.Find("LoserSpots").transform.GetChild(1).position.z, GameObject.Find("LoserSpots").transform.GetChild(0).position.z)), new Quaternion(0, 180, 0, transform.rotation.w));
                }
                else
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(GameObject.Find("LoserSpots").transform.GetChild(2).position.x, GameObject.Find("LoserSpots").transform.GetChild(3).position.x), transform.position.y, Random.Range(GameObject.Find("LoserSpots").transform.GetChild(3).position.z, GameObject.Find("LoserSpots").transform.GetChild(2).position.z)), new Quaternion(0, 180, 0, transform.rotation.w));
                }
            }
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(transform.GetChild(0).position.x, transform.GetChild(1).position.x), transform.position.y, transform.position.z), transform.rotation);
        }
    }
}
