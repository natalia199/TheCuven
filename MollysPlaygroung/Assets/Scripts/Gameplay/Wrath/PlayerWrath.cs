using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWrath : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed;

    Vector3 keyboardMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        keyboardMovement.x = Input.GetAxisRaw("Horizontal");
        keyboardMovement.z = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
