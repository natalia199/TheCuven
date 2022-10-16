using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Camera mainCam;
    public Transform doorSpawnPoint;
    public Transform roomLocaiton;

    public Transform changeRoomView()
    {
        mainCam.transform.position = new Vector3(roomLocaiton.position.x, roomLocaiton.position.y, -10f);
        return doorSpawnPoint;
    }
}
