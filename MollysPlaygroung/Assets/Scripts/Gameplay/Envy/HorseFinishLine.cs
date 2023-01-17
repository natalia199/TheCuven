using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseFinishLine : MonoBehaviour
{
    bool letsgo; 
    public Transform finishLinePoint;

    public int horseID;
    public string horsename;

    void Start()
    {
        letsgo = false;
        transform.parent.name = horsename;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(letsgo)
        {
            float step = 2f * Time.deltaTime; // calculate distance to move
            transform.GetChild(0).position = Vector3.MoveTowards(transform.position, finishLinePoint.position, step);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            letsgo = true;
            GameObject.Find("GameManager").GetComponent<EnvyGameManager>().UpdateRaceResult(transform.parent.name);
        }
    }
}
