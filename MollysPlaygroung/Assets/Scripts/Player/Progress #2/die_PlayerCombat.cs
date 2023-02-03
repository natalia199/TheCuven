using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class die_PlayerCombat : MonoBehaviour
{
    /// <summary>
    /// This script holds all the major actions needed for the player (walking/punching/pulling/getting hit/getting dragged/jumping)
    /// Each level will have these actions no matter what
    /// </summary>
    
    private die_PlayerMovement movementScript;
    [SerializeField] Animator animator;
    main_PlayerCombat opponentCombatState;

    int isWalkingHash;
    public bool isPunching = false;
    public bool isPulling = false;
    public bool isDragged = false;
    public bool isPushing = false;


    [SerializeField] float dragDistance;

    public bool stunned = false;

    int stunBreaker = 0;
    public bool freedom = false;
    float timeRemaining = 0;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
         isDragged = animator.GetBool("isDragged");


        // Controls            

        if (opponentCombatState != null)
        {
            // getting punchies
            if (opponentCombatState.isPunching)
            {
                GetComponent<die_PlayerCombat>().animator.SetTrigger("isHitTrigger");
                GetComponent<die_PlayerCombat>().Stun();
            }

            // being dragged
            else if (opponentCombatState.isPulling && !animator.GetCurrentAnimatorStateInfo(0).IsName("Being Dragged"))
            {
                //parenting to move the object with teh oponent
                Transform oppTransform = opponentCombatState.GetComponent<Transform>();

                Draggies(this.name, opponentCombatState.gameObject.name, oppTransform.position, oppTransform.rotation);
            }

            else if (opponentCombatState.isPushing)
            {
                GetComponent<die_PlayerCombat>().animator.SetTrigger("isHitTrigger");

                Vector3 force = transform.position - opponentCombatState.transform.position;
                force.Normalize();
                GetComponent<Rigidbody>().AddForce(force * 50);
            }
        }

        
    }


    /// <summary>
    /// 
    /// TO DO:
    /// clean up player
    /// make a WEBGL to test player actions
    /// if it goes well then start networking all 7 levels
    /// 
    /// </summary>
    /// <param name="other"></param>


    void OnTriggerStay(Collider other)
    {
        try
        {
            if (other.tag == "Player")
            {
                opponentCombatState = other.GetComponent<main_PlayerCombat>();
            }

        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            opponentCombatState = null;
        }
    }

    
    void Draggies(string thisPlayer, string playerDragging, Vector3 pos, Quaternion rot)
    {
        try
        {
            GameObject.Find(thisPlayer).transform.parent = GameObject.Find(playerDragging).transform;
            GameObject.Find(thisPlayer).transform.position = pos;
            GameObject.Find(thisPlayer).transform.rotation = rot;
            GameObject.Find(thisPlayer).transform.position += GameObject.Find(thisPlayer).transform.forward * dragDistance;
            GameObject.Find(thisPlayer).GetComponent<Rigidbody>().isKinematic = true;

            GameObject.Find(thisPlayer).transform.GetChild(0).GetComponent<Animator>().SetTrigger("isDraggedTrigger");
            GameObject.Find(thisPlayer).transform.GetChild(0).GetComponent<Animator>().SetBool("isDragged", true);

            GameObject.Find(thisPlayer).GetComponent<die_PlayerCombat>().isDragged = true;
            GameObject.Find(thisPlayer).GetComponent<die_PlayerCombat>().stunned = true;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    public void Stun()
    {
        StartCoroutine("StunPeriod", 5);
    }

    IEnumerator StunPeriod(int value)
    {
        stunned = true;

        yield return new WaitForSeconds(value);

        stunned = false;
    }
}
