using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerWrath_Combat : MonoBehaviour
{
    PhotonView view;

    [SerializeField] Animator animator;
    public bool isPunching = false;  //Maybe create a public instance of the isPunching animation state. This will be used to access the animation state over networking since the animation controller is localized to current machine?
    public bool isPulling = false;
    public bool isDragged = false;
    public bool isPushing = false;
    [SerializeField] float dragDistance = 1.5f;
    bool killme = false;
    private int isHitHash;
    public GameObject[] foodItems;
    //I implemented this if it's needed. Not really sure if that's useful or if the animator can be accessed by PUN

    PlayerWrath_Combat opponentCombatState;

    public bool theBitchIsStunned = false;

    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            isHitHash = Animator.StringToHash("isHit");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            isPunching = animator.GetBool("isPunching");
            isPulling = animator.GetBool("isPulling");
            isDragged = animator.GetBool("isDragged");
            isPushing = animator.GetBool("isPushing");

            if (killme)
            {
                //Check if the other player is punching

                if (opponentCombatState.isPunching)
                {
                    theBitchIsStunned = true;
                    GetComponent<PlayerWrath_ZachyNati>().CallStunah();
                    view.RPC("Ouchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                }
                else if (opponentCombatState.isPulling && !animator.GetCurrentAnimatorStateInfo(0).IsName("Being Dragged"))
                {
                    //animate the player being dragged
                    theBitchIsStunned = true; 
                    GetComponent<PlayerWrath_ZachyNati>().stunTheBitch = false;

                    animator.SetTrigger("isDraggedTrigger");
                    animator.SetBool("isDragged", true);

                    Transform oppTransform = opponentCombatState.gameObject.GetComponent<Transform>(); //transform of opponent
                    Transform currentTransform = GetComponent<Transform>();
                    currentTransform.SetPositionAndRotation(oppTransform.position, oppTransform.rotation);
                    currentTransform.position += currentTransform.forward * dragDistance;
                    Vector3 grr = currentTransform.position += currentTransform.forward * dragDistance;
                    //currentTransform.parent = oppTransform;

                    view.RPC("Draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, opponentCombatState.gameObject.name, grr, true);
                }
                else if (opponentCombatState.isPushing)
                {
                    //animate the player being hit

                    //push player back along direction it was hit
                    Transform oppTransform = opponentCombatState.gameObject.GetComponent<Transform>(); //transform of opponent
                    Transform currentTransform = GetComponent<Transform>();
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        //check if the colission is another player
        if (other.CompareTag("Player"))
        {
            killme = true;
            opponentCombatState = other.GetComponent<PlayerWrath_Combat>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        //check if the colission is another player
        if (other.CompareTag("Player"))
        {
            killme = false;
            opponentCombatState = null;
        }
    }

    [PunRPC]
    void Ouchies(string player)
    {
        try
        {
            Debug.Log("Ouchies " + player + " got hit");
            GameObject.Find(player).GetComponent<PlayerWrath_Combat>().animator.SetTrigger("isHitTrigger");
            //GameObject.Find(player).GetComponent<PlayerWrath_Combat>().theBitchIsStunned = true;
            //GameObject.Find(player).GetComponent<PlayerWrath_ZachyNati>().CallStunah();

            //GameObject.Find(player).GetComponent<AnimationWrath_ZachyNati>().resetAnimations();
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void Draggies(string player, string draggeePlayer, Vector3 DIE, bool state)
    {
        try
        {
            Debug.Log("parent " + player + " kiddie " + draggeePlayer);
            GameObject.Find(player).GetComponent<PlayerWrath_Combat>().isDragged = state;

            GameObject.Find(player).transform.parent = GameObject.Find(draggeePlayer).transform;
            GameObject.Find(player).GetComponent<Transform>().position = DIE;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
