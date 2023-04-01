using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundingCheck : MonoBehaviour
{
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            transform.parent.GetComponent<PlayerUserTest>().playerIsGrounded = true;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            transform.parent.GetComponent<PlayerUserTest>().playerIsGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            transform.parent.GetComponent<PlayerUserTest>().playerIsGrounded = false;
        }
    }
}
