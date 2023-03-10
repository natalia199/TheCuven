using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAnimationController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isDraggingHash;
    int isDraggedHash;
    int isChipWalkingHash;
    int isTrappedHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isDraggingHash = Animator.StringToHash("isDragging");
        isDraggedHash = Animator.StringToHash("isBeingDragged");
        isChipWalkingHash = Animator.StringToHash("isChipWalking");
        isTrappedHash = Animator.StringToHash("isTrapped");
    }

    // Update is called once per frame
    void Update()
    {

        bool isWalking = animator.GetBool("isWalking");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        bool directionPressed = inputHorizontal != 0 || inputVertical != 0;

        if (!isWalking && directionPressed)
        {
            if (Input.GetKey(KeyCode.LeftShift)) //holding shift toggles walk and chip walk
            {
                animator.SetBool(isChipWalkingHash, true);
            }
            else
            {
                animator.SetBool(isWalkingHash, true);
            }    
        }
        if (isWalking && !directionPressed)
        {
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isChipWalkingHash, false);
        }
        if (Input.GetKeyDown("e"))
        {
            animator.SetTrigger("AttackTrigger");
        }
        if (Input.GetKeyDown("r"))
        {
            animator.SetTrigger("HitTrigger");
        }
        if (Input.GetKey("t"))
        {
            animator.SetBool(isDraggingHash, true);
        }
        else
        {
            animator.SetBool(isDraggingHash, false);
        }
        if (Input.GetKey("y"))
        {
            animator.SetBool(isDraggedHash, true);
        }
        else
        {
            animator.SetBool(isDraggedHash, false);
        }
        if (Input.GetKeyDown("u"))
        {
            animator.SetTrigger("PushTrigger");
        }
        if (Input.GetKeyDown("i"))
        {
            animator.SetTrigger("PushedTrigger");
        }
        
        if (Input.GetKeyDown("space") && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetTrigger("PianoJumpTrigger");
            
        } else if (Input.GetKeyDown("space"))
        {
            animator.SetTrigger("JumpTrigger");
            
        }
        if (Input.GetKeyDown("o"))
        {
            animator.SetTrigger("ChipThrowTrigger");
        }
        if (Input.GetKeyDown("g"))
        {
            animator.SetTrigger("PoisonTrigger");
        }
        if (Input.GetKeyDown("h"))
        {
            animator.SetTrigger("SquirtTrigger");
        }
        if (Input.GetKeyDown("j"))
        {
            animator.SetTrigger("OpenTrapTrigger");
        }
        if (Input.GetKey("k"))
        {
            animator.SetBool(isTrappedHash, true);
        }
        else
        {
            animator.SetBool(isTrappedHash, false);
        }
        if (Input.GetKeyDown("l")) {
            animator.SetTrigger("sitTrigger");
        }
        if (Input.GetKeyDown("v"))
        {
            animator.SetTrigger("standTrigger");
        }

    }
}
