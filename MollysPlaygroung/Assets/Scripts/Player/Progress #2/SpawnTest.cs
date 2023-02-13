using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnTest : MonoBehaviour
{
    public GameObject playerPrefab;

    void Awake()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(transform.GetChild(0).position.x, transform.GetChild(1).position.x), transform.position.y, transform.position.z), Quaternion.identity);
    }
}
