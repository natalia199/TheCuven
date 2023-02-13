using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class playerMovement : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController charAnimations;

    PhotonView view;

    public float force;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Changing player img to selected character
        if (view.IsMine)
        {
            // HORIZONTALLY MOVING ANIMATION
            float hor = Input.GetAxisRaw("Horizontal") * force;
            float ver = Input.GetAxisRaw("Vertical") * force;

            Vector3 movement = new Vector3(hor, 0, ver).normalized;

            if (hor != 0 && ver == 0)
            {
                if (hor > 1)
                {
                    view.RPC("flipAnimation", RpcTarget.AllBufferedViaServer, "P" + PhotonNetwork.LocalPlayer.ActorNumber.ToString(), false);
                }
                else if (hor < -1)
                {
                    view.RPC("flipAnimation", RpcTarget.AllBufferedViaServer, "P" + PhotonNetwork.LocalPlayer.ActorNumber.ToString(), true);
                }


                GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + movement * force * Time.fixedDeltaTime);
                animator.SetFloat("Speed", Mathf.Abs(hor));
            }

            else if (ver != 0 && hor == 0)
            {
                GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + movement * force * Time.fixedDeltaTime);

                animator.SetFloat("verticalSpeed", ver);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                animator.SetFloat("Speed", Mathf.Abs(0));
                animator.SetFloat("verticalSpeed", 0);
            }
        }
    }

    [PunRPC]
    void flipAnimation(string pName, bool flip)
    {
        try
        {
            GameObject.Find(pName).GetComponent<SpriteRenderer>().flipX = flip;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
