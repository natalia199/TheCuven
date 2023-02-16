using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipScript : MonoBehaviour
{
    public bool Available;
    Rigidbody rb;

    void Start()
    {
        Available = true;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void throwChip(Vector3 targetPt, Vector3 playerPos, float force)
    {
        rb = GetComponent<Rigidbody>();
        throwParabola(targetPt, playerPos, force);
    }

    void MoveBoy(Vector3 vel)
    {
        rb.velocity = vel;
    }

    void throwParabola(Vector3 targetPos, Vector3 startPos, float force)
    {
        Vector3 dir = targetPos - startPos;
        dir = dir.normalized;
        GetComponent<Rigidbody>().AddForce(dir * force);
        GetComponent<Rigidbody>().AddTorque(dir * 50);
    }
}
