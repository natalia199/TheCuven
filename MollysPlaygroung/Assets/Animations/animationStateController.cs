using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{

    Animator animator;
    int isWalkingHash;
    int isPunchingHash;
    int isHitHash;
    int isPullingHash;
    public bool isTestDummy = false;
    [SerializeField] GluttonyLevel LevelController;
    private PlayerMovement3D movementScript;
    // Start is called before the first frame update

    //Allowing Toggleable animation properties depending on the needs of the level.
    void Start()
    {
        movementScript = GetComponentInParent<PlayerMovement3D>();
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isPunchingHash = Animator.StringToHash("isPunching");
        isHitHash = Animator.StringToHash("isHit");
        isPullingHash = Animator.StringToHash("isPulling");
    }

    // Update is called once per frame
    void Update()
    {

        
        if (!LevelController.isStartTimerOn)
        {
            if (!isTestDummy) //disabling punching and running for any testing characters.
            {

                bool isWalking = animator.GetBool("isWalking");
                bool isPunching = animator.GetBool("isPunching");
                bool isPulling = animator.GetBool("isPulling");
                bool isPushing = animator.GetBool("isPushing"); 

                float inputHorizontal = Input.GetAxisRaw("Horizontal");
                float inputVertical = Input.GetAxisRaw("Vertical");
                float inputPunch = Input.GetAxisRaw("Fire1");
                bool directionPressed = inputHorizontal != 0 || inputVertical != 0;
                bool punchingPressed = inputPunch != 0;
                bool pushingPressed = Input.GetAxisRaw("Fire3") != 0;

                bool pullingPressed = Input.GetAxisRaw("Fire2") != 0;
                if (!isWalking && directionPressed)
                {
                    animator.SetBool(isWalkingHash, true);
                }
                if (isWalking && !directionPressed)
                {
                    animator.SetBool(isWalkingHash, false);
                }
                if (!isPunching && punchingPressed)  
                {
                    animator.SetBool(isPunchingHash, true);
                }
                if (isPunching && !punchingPressed)
                {
                    animator.SetBool(isPunchingHash, false);
                }

                if (!isPulling && pullingPressed)
                {
                    animator.SetBool(isPullingHash, true);
                }
                if (isPulling && !pullingPressed)
                {
                    //Set back to false after the punch animation finishes
                    animator.SetBool(isPullingHash, false);
                    //find linked object and unparent it so that is is no longer "pulled"
                    Transform currentTransform = GetComponent<Transform>();
                    foreach (Transform child in currentTransform.parent)
                     {
                       
                        if (child.CompareTag("Player"))
                        {
                            child.transform.parent = null;
                            //Cancel the child elements dragged animation
                            Animator childAnim = child.GetComponentInChildren<Animator>();
                            childAnim.SetBool("isDragged", false);
                            break;
                            
                        }
                    }


                }
                //Jump Check
                Debug.Log(Input.GetButtonDown("Jump") && !movementScript.isJumping);
                Debug.Log(Input.GetButtonDown("Jump"));
                Debug.Log(!movementScript.isJumping);
                if (Input.GetButtonDown("Jump") && !movementScript.isJumping)
                {
                    animator.SetTrigger("isJumpingTrigger");
                }
                if (pushingPressed && !isPushing)
                {
                    animator.SetBool("isPushing", true);
                }
                if (!pushingPressed && isPushing)
                {
                    animator.SetBool("isPushing", false);
                }
            }
            //Actions for the dummies as well.
        }
        
    }
}
