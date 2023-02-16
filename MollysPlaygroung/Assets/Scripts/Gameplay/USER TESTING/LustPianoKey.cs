using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LustPianoKey : MonoBehaviour
{
    bool keyPressed = false;

    public float _rotateSpeed;
    public float _speed;

    public Color keyColour;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        if (keyPressed)
        {
            Debug.Log("get off");

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, _rotateSpeed * Time.deltaTime);

        }
        else
        {
            Debug.Log("on it");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            keyPressed = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            keyPressed = false;
        }
    }
}
