using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSides : MonoBehaviour
{
    public bool onGround;
    public int sideValue;

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "DiceGround")
        {
            onGround = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "DiceGround")
        {
            onGround = false;
        }
    }
}
