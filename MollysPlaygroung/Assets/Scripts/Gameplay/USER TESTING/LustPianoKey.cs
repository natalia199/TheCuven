using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LustPianoKey : MonoBehaviour
{
    public bool keyPressed = false;

    public bool activatedPianoKey = false;

    public float _rotateSpeed;
    public float _speed;

    public Material originalColour;
    public Material selectedColour;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        if (!keyPressed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, _rotateSpeed * Time.deltaTime);
        }
    }

    public void selectedKey()
    {
        GetComponent<MeshRenderer>().material = selectedColour;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = selectedColour;

        activatedPianoKey = true;
    }

    public void deselectedKey()
    {
        GetComponent<MeshRenderer>().material = originalColour;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = originalColour;

        activatedPianoKey = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            keyPressed = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            keyPressed = true;
        }
    }
}
