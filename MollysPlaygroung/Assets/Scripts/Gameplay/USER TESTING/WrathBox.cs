using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            if (playerInteracted != null)
            {
                playerInteracted.GetComponent<PlayerUserTest>().boxScore++;
                GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "Boxes: " + playerInteracted.GetComponent<PlayerUserTest>().boxScore;
            }

            GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().TrackBoxes--;

            Destroy(this.gameObject);
        }

        if (other.tag == "WrathPlatform")
        {
            playerInteracted = null;
        }

    }
}
