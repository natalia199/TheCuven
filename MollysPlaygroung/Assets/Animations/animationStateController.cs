using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{

    Animator animator;
    int isWalkingHash;
    int isPunchingHash;
    int isHitHash;
    public bool isTestDummy = false;
    [SerializeField] GluttonyLevel LevelController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isPunchingHash = Animator.StringToHash("isPunching");
        isHitHash = Animator.StringToHash("isHit");
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
                


                float inputHorizontal = Input.GetAxisRaw("Horizontal");
                float inputVertical = Input.GetAxisRaw("Vertical");
                float inputPunch = Input.GetAxisRaw("Fire1");
                bool directionPressed = inputHorizontal != 0 || inputVertical != 0;
                bool punchingPressed = inputPunch != 0;
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
                    Debug.Log("punch");
                    animator.SetBool(isPunchingHash, true);
                }
                if (isPunching && !punchingPressed)
                {
                    //Debug.Log("stoping punching");
                    //Set back to false after the punch animation finishes
                    animator.SetBool(isPunchingHash, false);
                }
            }
            //Actions for everyone
        }
        
    }
}
