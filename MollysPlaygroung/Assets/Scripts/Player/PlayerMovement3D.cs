using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public CharacterController controller;
    public Rigidbody rigidBody;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rigidBody.velocity = new Vector3(horizontalInput * speed, rigidBody.velocity.y, verticalInput * speed);

        if (Input.GetButtonDown("Jump"))
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
        }
    }

    float getVelocity(string axis)
    {
        switch (axis) {
            case "x":
                return rigidBody.velocity.x;
            case "y":
                return rigidBody.velocity.y;
            case "z":
                return rigidBody.velocity.z;
        }

        return 0.0f;
        
    }
}
