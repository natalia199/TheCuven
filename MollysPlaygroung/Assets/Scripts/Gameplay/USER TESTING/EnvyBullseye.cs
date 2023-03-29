using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvyBullseye : MonoBehaviour
{
    public bool Bullseye = false;

    // Water touches the target
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "squirtTarget")
        {
            Bullseye = true;
        }
    }

    // Water isn't touching the target
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "squirtTarget")
        {
            Bullseye = false;
        }
    }
}
