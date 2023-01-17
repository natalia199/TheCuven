using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseMovement : MonoBehaviour
{
    public Transform finishLinePoint;

    public bool race;
    public bool finished;

    public string horseName;

    public float thpeed;

    void Start()
    {
        thpeed = 0;
        this.name = horseName;
        race = false;
    }

    void FixedUpdate()
    {
        if (race || finished)
        {
            float step = 2f * Time.deltaTime; // calculate distance to move
            transform.GetChild(0).position = Vector3.MoveTowards(transform.GetChild(0).position, finishLinePoint.position, thpeed);
        }
    }

    public void SetSpeed( float s)
    {
        thpeed = s;
    }
}
