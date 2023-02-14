using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvyHorse : MonoBehaviour
{
    public Transform finishLinePoint;

    public bool finished;
    public float thpeed;

    public float speed = 1;
    public float RotAngleY;

    void Start()
    {
        finished = false;
    }

    void FixedUpdate()
    {
        if (!finished && Input.GetKey(KeyCode.Space))
        {
            transform.position = Vector3.MoveTowards(transform.position, finishLinePoint.position, thpeed);

            float rY = Mathf.SmoothStep(-RotAngleY, RotAngleY,Mathf.PingPong(Time.time * speed, 1));
            transform.rotation = Quaternion.Euler(0,0,rY);
        }
        else if(finished)
        {
            transform.position = Vector3.MoveTowards(transform.position, finishLinePoint.position, thpeed);
            float rY = Mathf.SmoothStep(-RotAngleY, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
            transform.rotation = Quaternion.Euler(0, 0, rY);
        }
        else
        {
            float rY = Mathf.SmoothStep(0, 0, Mathf.PingPong(Time.time * speed, 1));
            transform.rotation = Quaternion.Euler(0, 0, rY);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            finished = true;
        }
    }
}