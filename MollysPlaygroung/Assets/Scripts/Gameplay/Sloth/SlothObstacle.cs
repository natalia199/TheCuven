using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlothObstacle : MonoBehaviour
{
    public bool trapSet;

    public GameObject caughtPlayer;

    void Start()
    {
        trapSet = false;
    }

    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        // GREED

        if (other.tag == "Player" && !trapSet)
        {
            caughtPlayer = other.gameObject;
        }
    }
}
