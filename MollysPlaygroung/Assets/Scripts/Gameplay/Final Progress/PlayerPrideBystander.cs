using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrideBystander : MonoBehaviour
{

    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {

    }

}
