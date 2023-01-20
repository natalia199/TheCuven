using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    bool change;
    // Start is called before the first frame update
    void Start()
    {
        change = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value > 0.99f && !change) //a random chance
        {
            GetComponent<Light>().enabled = false; //turn it off
            change = true;
        }
        else if (Random.value > 0.9f && change)
        {
            GetComponent<Light>().enabled = true; //turn it on
            change = false;
        }
    }
}
