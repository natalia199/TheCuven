using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedChip : MonoBehaviour
{
    public bool availableForCarry;
    Rigidbody rb;

    void Start()
    {
        availableForCarry = true;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void throwChip(Vector3 targetPt, Vector3 playerPos)
    {
        rb = GetComponent<Rigidbody>();
        throwParabola(targetPt, playerPos);
    }

    void MoveBoy(Vector3 vel)
    {
        rb.velocity = vel;
    }

    void throwParabola(Vector3 targetPos, Vector3 startPos)
    {
        Vector3 dir = targetPos - startPos;
        dir = dir.normalized;
        GetComponent<Rigidbody>().AddForce(dir * 300);
    }
}