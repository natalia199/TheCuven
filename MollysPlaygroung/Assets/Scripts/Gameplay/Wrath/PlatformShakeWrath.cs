using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShakeWrath : MonoBehaviour
{
    public float speed = 6f;

    List<Vector3> idek = new List<Vector3>();

    int directionIndex;

    public bool startTilt;
    public bool returnHome;
    public bool popOut;

    void Start()
    {
        startTilt = false;
        returnHome = false;
        popOut = false;

        idek.Add(Vector3.left);
        idek.Add(Vector3.right);
        idek.Add(Vector3.forward);
        idek.Add(Vector3.back);

        GoOffQueen();
    }

    void FixedUpdate()
    {
        if (startTilt)
        {
            transform.Rotate(idek[directionIndex], speed * Time.deltaTime);
        }
        else if (returnHome)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, Time.deltaTime * 15f);
        }

        
        if (returnHome && transform.rotation == Quaternion.identity)
        {
            returnHome = false;
            GoOffQueen();
        }
        
    }

    void GoOffQueen()
    {
        StartCoroutine("ChangeTilt");
    }

    IEnumerator ChangeTilt()
    {
        directionIndex = Random.Range(0, 3);
        startTilt = true;

        float timez = Random.Range(2, 8);

        if (!startTilt)
        {
            yield break;
        }

        yield return new WaitForSeconds(timez);

        if (startTilt)
        {
            startTilt = false;
            GoOffQueen();
        }
    }

    IEnumerator OriginalTilt()
    {
        returnHome = true;

        yield return new WaitForSeconds(0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OffLimitsWrath")
        {
            popOut = true;
            startTilt = false;
            StartCoroutine("OriginalTilt");
        }
    }
}
