using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class AnimationEnvy_ZachyNati : MonoBehaviour
{
    PhotonView view;

    public Animator animator;
    int isWalkingHash;
    int isPunchingHash;
    int isHitHash;
    int isPullingHash;
    public bool isTestDummy = false;
    private PlayerEnvy_ZachyNati movementScript;
    // Start is called before the first frame update

    //Allowing Toggleable animation properties depending on the needs of the level.
    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            movementScript = GetComponent<PlayerEnvy_ZachyNati>();
            isWalkingHash = Animator.StringToHash("isWalking");
            isPunchingHash = Animator.StringToHash("isPunching");
            isHitHash = Animator.StringToHash("isHit");
            isPullingHash = Animator.StringToHash("isPulling");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine) //disabling punching and running for any testing characters.
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
                //view.RPC("Walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                animator.SetBool(isWalkingHash, true);
            }
            if (isWalking && !directionPressed)
            {
                //view.RPC("Walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                animator.SetBool(isWalkingHash, false);
            }
            if (!isPunching && punchingPressed)
            {
                view.RPC("Punchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                animator.SetBool(isPunchingHash, true);
            }
            if (isPunching && !punchingPressed)
            {
                view.RPC("Punchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                animator.SetBool(isPunchingHash, false);
            }

            if (!isPulling && pullingPressed)
            {
                view.RPC("Pullies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                animator.SetBool(isPullingHash, true);
            }
            if (isPulling && !pullingPressed)
            {
                //Set back to false after the punch animation finishes
                view.RPC("Pullies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                animator.SetBool(isPullingHash, false);
                //find linked object and unparent it so that is is no longer "pulled"
                for(int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).tag == "Player")
                    {
                        view.RPC("Draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, transform.GetChild(i).name, false);

                        break;
                    }
                }
            }
            //Jump Check
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


    [PunRPC]
    void Pullies(string player, bool state)
    {
        try
        {
            GameObject.Find(player).GetComponent<PlayerEnvy_Combat>().isPulling = state;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void Punchies(string player, bool state)
    {
        try
        {
            GameObject.Find(player).GetComponent<PlayerEnvy_Combat>().isPunching = state;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void Draggies(string player, string draggedPlayer, bool state)
    {
        try
        {
            GameObject.Find(draggedPlayer).GetComponent<PlayerEnvy_Combat>().isDragged = state;
            GameObject.Find(draggedPlayer).GetComponentInChildren<Animator>().SetBool("isDragged", false);
            GameObject.Find(draggedPlayer).transform.parent = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
