using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvyBullseye : MonoBehaviour
{
    public bool Bullseye = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "squirtTarget")
        {
            Bullseye = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "squirtTarget")
        {
            Bullseye = false;
        }
    }
}
