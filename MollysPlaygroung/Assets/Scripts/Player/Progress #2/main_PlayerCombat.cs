using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class main_PlayerCombat : MonoBehaviour
{
    /// <summary>
    /// This script holds all the major actions needed for the player (walking/punching/pulling/getting hit/getting dragged/jumping)
    /// Each level will have these actions no matter what
    /// </summary>
    
    PhotonView view;

    private main_PlayerMovement movementScript;
    [SerializeField] Animator animator;
    main_PlayerCombat opponentCombatState;

    int isWalkingHash;
    public bool isPunching = false;
    public bool isPulling = false;
    public bool isDragged = false;

    [SerializeField] float dragDistance = 1.5f;


    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            movementScript = GetComponent<main_PlayerMovement>();
            isWalkingHash = Animator.StringToHash("isWalking");
        }
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            isPunching = animator.GetBool("isPunching");
            isPulling = animator.GetBool("isPulling");
            isDragged = animator.GetBool("isDragged");
            bool isWalkingCheck = animator.GetBool("isWalking");

            // Controls
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            float inputVertical = Input.GetAxisRaw("Vertical");
            float inputPunch = Input.GetAxisRaw("Fire1");
            bool directionPressed = inputHorizontal != 0 || inputVertical != 0;
            bool punchingPressed = inputPunch != 0;
            bool pullingPressed = Input.GetAxisRaw("Fire2") != 0;

            // Walking
            if (!isWalkingCheck && directionPressed)
            {
                animator.SetBool(isWalkingHash, true);
            }
            if (isWalkingCheck && !directionPressed)
            {
                animator.SetBool(isWalkingHash, false);
            }

            //Jump Check
            Debug.Log(Input.GetButtonDown("Jump"));
            if (Input.GetButtonDown("Jump") && !movementScript.isJumping)
            {
                view.RPC("Jumpy", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
            }

            // Punching
            if (!isPunching && punchingPressed)
            {
                view.RPC("Punchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
            }
            if (isPunching && !punchingPressed)
            {
                view.RPC("Punchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
            }

            // Pulling
            if (!isPulling && pullingPressed)
            {
                view.RPC("Pullies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
            }
            if (isPulling && !pullingPressed)
            {
                view.RPC("Pullies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).tag == "Player")
                    {
                        view.RPC("Releasies", RpcTarget.AllBufferedViaServer, transform.GetChild(i).name);
                        break;
                    }
                }
            }

            if (opponentCombatState != null)
            {
                // punchies
                if (opponentCombatState.isPunching)
                {
                    view.RPC("Ouchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                }

                // being dragged
                else if (opponentCombatState.isPulling && !animator.GetCurrentAnimatorStateInfo(0).IsName("Being Dragged"))
                {
                    //parenting to move the object with teh oponent
                    Transform oppTransform = opponentCombatState.GetComponent<Transform>();

                    view.RPC("Draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, opponentCombatState.gameObject.name, oppTransform.position, oppTransform.rotation);
                }
            }
        }
    }


    /// <summary>
    /// 
    /// TO DO:
    /// play with zachs drag code
    /// clean up player
    /// make a WEBGL to test player actions
    /// if it goes well then start networking all 7 levels
    /// 
    /// </summary>
    /// <param name="other"></param>


    void OnTriggerStay(Collider other)
    {
        // photonnetwork doesnt like this!!!!!!!!!!!
        if (view.IsMine)
        {
            if (other.tag == "Player")
            {
                opponentCombatState = other.GetComponent<main_PlayerCombat>();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            opponentCombatState = null;
        }
    }

    /// NETWORKING 
    
    // jumpies
    [PunRPC]
    void Jumpy(string player)
    {
        try
        {
            GameObject.Find(player).transform.GetChild(0).GetComponent<Animator>().SetTrigger("isJumpingTrigger");
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // ouchies
    [PunRPC]
    void Ouchies(string player)
    {
        try
        {
            GameObject.Find(player).GetComponent<main_PlayerCombat>().animator.SetTrigger("isHitTrigger");
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // being dragged rip
    [PunRPC]
    void Draggies(string thisPlayer, string playerDragging, Vector3 pos, Quaternion rot)
    {
        try
        {
            GameObject.Find(thisPlayer).transform.GetChild(0).GetComponent<Animator>().SetTrigger("isDraggedTrigger");
            GameObject.Find(thisPlayer).transform.GetChild(0).GetComponent<Animator>().SetBool("isDragged", true);

            GameObject.Find(thisPlayer).transform.position = pos;
            GameObject.Find(thisPlayer).transform.rotation = rot;
            GameObject.Find(thisPlayer).transform.position += GameObject.Find(thisPlayer).transform.forward * dragDistance;
            GameObject.Find(thisPlayer).transform.parent = GameObject.Find(playerDragging).transform;

            GameObject.Find(thisPlayer).GetComponent<main_PlayerCombat>().isDragged = true;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // punchies
    [PunRPC]
    void Punchies(string player, bool state)
    {
        try
        {
            GameObject.Find(player).transform.GetChild(0).GetComponent<Animator>().SetBool("isPunching", state);
            GameObject.Find(player).GetComponent<main_PlayerCombat>().isPunching = state;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // draggies
    [PunRPC]
    void Pullies(string player, bool state)
    {
        try
        {
            GameObject.Find(player).transform.GetChild(0).GetComponent<Animator>().SetBool("isPulling", state);
            GameObject.Find(player).GetComponent<main_PlayerCombat>().isPulling = state;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // releasing the draggie
    [PunRPC]
    void Releasies(string releasedPlayer)
    {
        try
        {
            GameObject.Find(releasedPlayer).transform.parent = null;
            GameObject.Find(releasedPlayer).transform.GetChild(0).GetComponent<Animator>().SetBool("isDragged", false);
            GameObject.Find(releasedPlayer).GetComponent<main_PlayerCombat>().isDragged = false;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
