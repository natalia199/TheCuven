using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{

    //[SerializeField] GluttonyLevel LevelController;
    private bool isGameStarted;
    private Rigidbody rigidBody;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 8f;
    PlayerCombat combatState;
    public bool isJumping = false;

    public float fallMultiplier = 5f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        combatState = GetComponent<PlayerCombat>();
    }

    void Update()
    {
        //isGameStarted = !LevelController.isStartTimerOn;

        if (isGameStarted)
        {
            MovePlayer();
            Jump();
        }
        

    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
            isJumping = true;
        }
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //fallMultipler - 1 accounts for build in gravity mutliplier
            isJumping = false;
        }
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        var isometricOffset = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        float moveSpeed = speed; //Making this a seperate variable to make speed adjustments for different animations

        Vector3 movement = isometricOffset.MultiplyPoint3x4(new Vector3(moveHorizontal, 0.0f, moveVertical));
        if(movement != Vector3.zero)
        {
            //flip looking direction if pulling to simulate walking backward
            if (combatState.isPulling || combatState.isDragged)
            {
                transform.rotation = Quaternion.LookRotation(-movement);
                moveSpeed = 3;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(movement);
            }
        }
        

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

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

    public bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }
}
