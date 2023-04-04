using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelResult : MonoBehaviour
{
    public string result;

    void OnTriggerStay(Collider other)
    {
        result = other.name;
    }
}