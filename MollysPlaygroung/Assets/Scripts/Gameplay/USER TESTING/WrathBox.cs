using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathBox : MonoBehaviour
{
    public GameObject playerInteracted;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // GREED

        if (other.tag == "Player")
        {
            playerInteracted = other.gameObject;
        }

        if (other.tag == "OffLimitsWrath")
        {
            playerInteracted.GetComponent<PlayerUserTest>().boxScore = (playerInteracted.GetComponent<PlayerUserTest>().boxScore + 1) / 2;
            Debug.Log("box score "+ playerInteracted.GetComponent<PlayerUserTest>().boxScore);
            Destroy(this.gameObject);
        }

    }
}
