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
    //die_PlayerCombat opponentCombatState;

    //Audio
    [SerializeField] AudioSource punchSFX;

    int isWalkingHash;
    public bool isPunching = false;
    public bool isPulling = false;
    public bool isDragged = false;
    public bool isPushing = false;

    bool urdone = false;

    [SerializeField] float dragDistance = 1.5f;

    public bool stunned = false;

    int stunBreaker = 0;
    public bool freedom = false;
    float timeRemaining = 0;

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

    void Update()
    {
        if (isDragged)
        {
            if (timeRemaining < 5)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    stunBreaker++;
                }

                timeRemaining += Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                stunBreaker = 0;
            }

            if(stunBreaker >= 5)
            {
                freedom = true;
            }
        }
        else
        {
            timeRemaining = 0;
            stunBreaker = 0;
        }
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            isPunching = animator.GetBool("isPunching");
            isPulling = animator.GetBool("isPulling");
            isDragged = animator.GetBool("isDragged");
            isPushing = animator.GetBool("isPushing");
            bool isWalkingCheck = animator.GetBool("isWalking");

            // Controls
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            float inputVertical = Input.GetAxisRaw("Vertical");
            float inputPunch = Input.GetAxisRaw("Fire1");
            bool directionPressed = inputHorizontal != 0 || inputVertical != 0;
            bool punchingPressed = inputPunch != 0;
            bool pullingPressed = Input.GetAxisRaw("Fire2") != 0;
            bool pushingPressed = Input.GetAxisRaw("Fire3") != 0;

            if (!stunned)
            {
                // Walking
                if (!isWalkingCheck && directionPressed)
                {
                    animator.SetBool(isWalkingHash, true);
                }
                if (isWalkingCheck && !directionPressed && !isPulling)
                {
                    animator.SetBool(isWalkingHash, false);
                }

                //Jump Check
                Debug.Log(Input.GetKeyDown(KeyCode.Space));
                if (Input.GetKeyDown(KeyCode.Space) && GetComponent<main_PlayerMovement>().isGrounded())
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

                // Pushing
                if (pushingPressed && !isPushing)
                {
                    view.RPC("Pushies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                }
                if (!pushingPressed && isPushing)
                {
                    view.RPC("Pushies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
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
            }

            if (isDragged && freedom)
            {
                view.RPC("Pullies", RpcTarget.AllBufferedViaServer, transform.parent.name, false);
                view.RPC("Releasies", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
            }

            if (opponentCombatState != null)
            {
                // getting punchies
                if (opponentCombatState.isPunching)
                {
                    view.RPC("Ouchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                }

                // being dragged
                else if (opponentCombatState.isPulling && !animator.GetCurrentAnimatorStateInfo(0).IsName("Being Dragged")&& !freedom)
                {
                    //parenting to move the object with teh oponent
                    Transform oppTransform = opponentCombatState.GetComponent<Transform>();

                    view.RPC("Draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, opponentCombatState.gameObject.name, oppTransform.position, oppTransform.rotation);
                }

                else if (opponentCombatState.isPushing)
                {
                    GetComponent<main_PlayerCombat>().animator.SetTrigger("isHitTrigger");

                    Vector3 force = transform.position - opponentCombatState.transform.position;
                    force.Normalize();
                    GetComponent<Rigidbody>().AddForce(force * 50);
                }
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
            // photonnetwork doesnt like this!!!!!!!!!!!
            if (view.IsMine && PhotonNetwork.IsConnected)
            {
                if (other.tag == "Player")
                {
                    opponentCombatState = other.GetComponent<main_PlayerCombat>();
                    //opponentCombatState = other.GetComponent<die_PlayerCombat>();
                }
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
            GameObject.Find(player).GetComponent<main_PlayerCombat>().Stun();
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
            GameObject.Find(thisPlayer).transform.parent = GameObject.Find(playerDragging).transform;
            GameObject.Find(thisPlayer).transform.position = pos;
            GameObject.Find(thisPlayer).transform.rotation = rot;
            GameObject.Find(thisPlayer).transform.position += GameObject.Find(thisPlayer).transform.forward * dragDistance;
            GameObject.Find(thisPlayer).GetComponent<Rigidbody>().isKinematic = true;

            GameObject.Find(thisPlayer).transform.GetChild(0).GetComponent<Animator>().SetTrigger("isDraggedTrigger");
            GameObject.Find(thisPlayer).transform.GetChild(0).GetComponent<Animator>().SetBool("isDragged", true);

            GameObject.Find(thisPlayer).GetComponent<main_PlayerCombat>().isDragged = true;
            GameObject.Find(thisPlayer).GetComponent<main_PlayerCombat>().stunned = true;
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
            Debug.Log("punchaz");
            GameObject.Find(player).GetComponent<main_PlayerCombat>().animator.SetBool("isWalking", false);
            GameObject.Find(player).GetComponent<main_PlayerCombat>().animator.SetBool("isPunching", state);
            //GameObject.Find(player).transform.GetChild(0).GetComponent<Animator>().SetBool("isPunching", state);
            GameObject.Find(player).GetComponent<main_PlayerCombat>().isPunching = state;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // pushies
    [PunRPC]
    void Pushies(string player, bool state)
    {
        try
        {
            GameObject.Find(player).transform.GetChild(0).GetComponent<Animator>().SetBool("isPushing", state);
            GameObject.Find(player).GetComponent<main_PlayerCombat>().isPushing = state;
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
            //GameObject.Find(player).GetComponent<die_PlayerCombat>().isPulling = state;
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
            GameObject.Find(releasedPlayer).GetComponent<main_PlayerCombat>().freedom = false;
            GameObject.Find(releasedPlayer).transform.parent = null;
            GameObject.Find(releasedPlayer).GetComponent<Rigidbody>().isKinematic = false;
            GameObject.Find(releasedPlayer).transform.GetChild(0).GetComponent<Animator>().SetBool("isDragged", false);
            GameObject.Find(releasedPlayer).GetComponent<main_PlayerCombat>().isDragged = false;
            GameObject.Find(releasedPlayer).GetComponent<main_PlayerCombat>().stunned = false;

            //GameObject.Find(releasedPlayer).GetComponent<die_PlayerCombat>().isDragged = false;
            //GameObject.Find(releasedPlayer).GetComponent<die_PlayerCombat>().stunned = false;
            //GameObject.Find(releasedPlayer).GetComponent<die_PlayerCombat>().freedom = false;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // releasing the draggie
    [PunRPC]
    void BreakOutStun(string releasedPlayer)
    {
        try
        {
            GameObject.Find(releasedPlayer).GetComponent<main_PlayerCombat>().freedom = true;
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
