using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseFinishLine : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            transform.parent.GetComponent<HorseMovement>().finished = true;
            GameObject.Find("GameManager").GetComponent<EnvyGameManager>().UpdateRaceResult(transform.parent.name);
        }
    }
}
