using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomPlayerSpawn : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        // Syncs all players when changing a scene
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }
}
