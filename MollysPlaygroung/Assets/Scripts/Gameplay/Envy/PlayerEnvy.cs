using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnvy : MonoBehaviour
{
    bool atShootingPad;

    public string horseName;

    Rigidbody rb;
    public float moveSpeed;

    Vector3 keyboardMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        atShootingPad = true;
    }

    void Update()
    {
        keyboardMovement.x = Input.GetAxisRaw("Horizontal");
        keyboardMovement.z = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && atShootingPad)
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameManager>().MoveHorse(horseName);
        }
        else
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameManager>().StopHorse(horseName);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == horseName && other.tag == "Horse")
        {
            atShootingPad = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.name == horseName && other.tag == "Horse")
        {
            atShootingPad = false;
        }
    }
}
