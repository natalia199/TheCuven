using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationMovementTest : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField] float speed = 5f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
            MovePlayer();


    }
    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        var isometricOffset = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        float moveSpeed = speed; //Making this a seperate variable to make speed adjustments for different animations

        Vector3 movement = isometricOffset.MultiplyPoint3x4(new Vector3(moveHorizontal, 0.0f, moveVertical));
        if (movement != Vector3.zero)
        {
            //flip looking direction if pulling to simulate walking backward
            if (false)//combatState.isPulling || combatState.isDragged)
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
}
