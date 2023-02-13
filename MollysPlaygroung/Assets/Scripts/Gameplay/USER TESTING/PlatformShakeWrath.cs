using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShakeWrath : MonoBehaviour
{
    public float speed = 6f;
    public float homeSpeed = 15f;
    public float homeHold = 3f;

    List<Vector3> idek = new List<Vector3>();

    public int directionIndex;
    public bool returnHome;
    bool newTilt;

    void Start()
    {
        newTilt = false;
        returnHome = false;

        idek.Add(Vector3.left);
        idek.Add(-Vector3.back);
        idek.Add(Vector3.back);

        StartCoroutine("HoldIt", homeHold);
    }

    void FixedUpdate()
    {
        if (newTilt)
        {
            directionIndex = Random.Range(0, 3);
            newTilt = false;
        }
        else if (returnHome)
        {
            if(transform.rotation == Quaternion.identity)
            {
                StartCoroutine("HoldIt", homeHold);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, Time.deltaTime * homeSpeed);
            }
        }
        else if(!newTilt && !returnHome)
        {
            transform.Rotate(idek[directionIndex], speed * Time.deltaTime);
        }
    }

    IEnumerator HoldIt(float time)
    {
        yield return new WaitForSeconds(time);

        newTilt = true;
        returnHome = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OffLimitsWrath")
        {
            returnHome = true;
        }
    }

    public void speedIncrease(float inc)
    {
        // Tilt speed
        speed += inc;

        // Return to normal speed
        homeSpeed += inc;

        // Time held at normal level
        homeHold += inc;
    }
}
