using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(transform.GetChild(0).position.x, transform.GetChild(1).position.x), transform.position.y, transform.position.z), transform.rotation);
    }
}
