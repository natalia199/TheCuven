using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathPlatformDetection : MonoBehaviour
{
    bool oneTime = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!oneTime)
            {
                //GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().RecordWrathResults(other.gameObject);
                oneTime = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            oneTime = false;
        }
    }
}