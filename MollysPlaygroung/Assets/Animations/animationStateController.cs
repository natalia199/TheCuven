using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{

    Animator animator;
    int isWalkingHash;
    [SerializeField] GluttonyLevel LevelController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    // Update is called once per frame
    void Update()
    {

        if (!LevelController.isStartTimerOn)
        {
            bool isWalking = animator.GetBool("isWalking");

            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            float inputVertical = Input.GetAxisRaw("Vertical");
            bool directionPressed = inputHorizontal != 0 || inputVertical != 0;

            if (!isWalking && directionPressed)
            {
                animator.SetBool(isWalkingHash, true);
            }
            if (isWalking && !directionPressed)
            {
                animator.SetBool(isWalkingHash, false);
            }
        }
        
    }
}
