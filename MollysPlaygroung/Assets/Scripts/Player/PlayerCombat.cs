using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] Animator animator;
    public bool isPunching = false;  //Maybe create a public instance of the isPunching animation state. This will be used to access the animation state over networking since the animation controller is localized to current machine?
    private int isHitHash;
                                    //I implemented this if it's needed. Not really sure if that's useful or if the animator can be accessed by PUN

    void Start()
    {
        isHitHash = Animator.StringToHash("isHit");
    }

    // Update is called once per frame
    void Update()
    {
        isPunching = animator.GetBool("isPunching");
    }

    private void OnTriggerStay(Collider other)
    {
        //check if the colission is another player
        if (other.CompareTag("Player"))
        {
            //Check if the other player is punching
            PlayerCombat opponentCombatState = other.GetComponentInParent<PlayerCombat>();
            if (opponentCombatState.isPunching && !animator.GetCurrentAnimatorStateInfo(0).IsName("Vomit"))
            {

                //change animation state
                animator.SetBool(isHitHash, true);
                animator.SetTrigger("isHitTrigger");
                Debug.Log("Puking CombatScript");
            }
        }


    }
}
