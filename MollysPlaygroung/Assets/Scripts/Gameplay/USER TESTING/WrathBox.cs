using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WrathBox : MonoBehaviour
{
    public GameObject playerInteracted;

    bool offPlatform = false;

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
            if (playerInteracted != null && !offPlatform)
            {
                //playerInteracted.GetComponent<PlayerUserTest>().boxScore++;
                GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().trackedBoxScore++;
                offPlatform = true;
            }

            //GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().maxAmountOfBoxes--;
            GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().TrackBoxes--;
            GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().boxesInstantiated++;

            Destroy(this.gameObject);
        }

        if (other.tag == "WrathPlatform")
        {
            playerInteracted = null;
        }

    }
}
