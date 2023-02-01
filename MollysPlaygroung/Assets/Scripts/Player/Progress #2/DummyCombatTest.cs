using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class DummyCombatTest : MonoBehaviour
{
    PhotonView view;

    int isWalkingHash;

    private MoveTest movementScript;
    [SerializeField] Animator animator;

    public bool isPunching = false;
    public bool isPulling = false;
    public bool isDragged = false;

    [SerializeField] float dragDistance = 1.5f;

    GameObject collidedOpponentCombatState;

    void Start()
    {
        view = GetComponent<PhotonView>();
        collidedOpponentCombatState = null;

        if (view.IsMine)
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            movementScript = GetComponent<MoveTest>();
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

            //Jump Check
            Debug.Log(Input.GetButtonDown("Jump"));
            if (Input.GetButtonDown("Jump") && !movementScript.isJumping)
            {
                view.RPC("Jumpy", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
            }

            // Being dragged
            if (collidedOpponentCombatState != null)
            {
                //Check if the other player is punching
                DummyCombatTest opponentCombatState = collidedOpponentCombatState.GetComponent<DummyCombatTest>();

                if (opponentCombatState.isPunching)
                {
                    view.RPC("Ouchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                }
                else if (opponentCombatState.isPulling && !animator.GetCurrentAnimatorStateInfo(0).IsName("Being Dragged"))
                {
                    //parenting to move the object with teh oponent
                    Transform oppTransform = collidedOpponentCombatState.GetComponent<Transform>();

                    view.RPC("Draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, opponentCombatState.gameObject.name, oppTransform.position, oppTransform.rotation);
                }
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colliding with " + other.name);
            collidedOpponentCombatState = GameObject.Find(other.name).gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("No longer colliding with " + other.name);
        if (other.CompareTag("Player"))
        {
            collidedOpponentCombatState = null;
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
            GameObject.Find(player).transform.GetChild(0).GetComponent<Animator>().SetTrigger("isHitTrigger");
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

            GameObject.Find(thisPlayer).transform.SetPositionAndRotation(pos, rot);
            GameObject.Find(thisPlayer).transform.position += GameObject.Find(thisPlayer).transform.forward * dragDistance;
            GameObject.Find(thisPlayer).transform.parent = GameObject.Find(playerDragging).transform;
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
        Debug.Log("yeur punch " + player);

        try
        {
            GameObject.Find(player).transform.GetChild(0).GetComponent<Animator>().SetBool("isPunching", state);
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
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
