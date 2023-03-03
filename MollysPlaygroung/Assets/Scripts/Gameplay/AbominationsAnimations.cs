using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbominationsAnimations : MonoBehaviour
{
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            bool isWalking = transform.GetChild(2).GetChild(0).GetComponent<Animator>().GetBool("isWalking");
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            float inputVertical = Input.GetAxisRaw("Vertical");
            bool directionPressed = inputHorizontal != 0 || inputVertical != 0;


            if (Input.GetKeyDown(KeyCode.I))
            {
                view.RPC("pushies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
            }
            else
            {
                view.RPC("pushies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                view.RPC("hitties", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
            }
            else
            {
                view.RPC("hitties", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
            }            
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                view.RPC("draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
            }
            else
            {
                view.RPC("draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
            }


            if (!GetComponent<PlayerUserTest>().freezePlayer)
            {
                if (SceneManager.GetActiveScene().name == "Greed")
                {
                    // walking
                    if (!isWalking && directionPressed)
                    {

                        if (GetComponent<PlayerUserTest>().collectedChipies.Count > 0) //holding shift toggles walk and chip walk
                        {
                            view.RPC("chipWalkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                        }
                        else
                        {
                            view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                            view.RPC("chipWalkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                        }
                    }

                    if (isWalking && !directionPressed)
                    {
                        view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                    }
                }
                else
                {
                    // walking
                    if (!isWalking && directionPressed)
                    {

                        view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);

                    }

                    if (isWalking && !directionPressed)
                    {
                        view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                    }
                }
            }
            else
            {
                view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
            }


            if (SceneManager.GetActiveScene().name == "Greed")
            {
               
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // throw
                    view.RPC("chipThrowies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                }
                else
                {
                    view.RPC("chipThrowies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                }
            }
            else if (SceneManager.GetActiveScene().name == "Envy")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (GetComponent<PlayerUserTest>().squirtAccess && GetComponent<PlayerUserTest>().squirtGun != null)
                    {
                        if (GetComponent<PlayerUserTest>().squirtGun.name == GetComponent<PlayerUserTest>().squirtGunName)
                        {
                            view.RPC("squirt", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                        }
                    }
                    else
                    {
                        view.RPC("squirt", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                    }
                }
                else
                {
                    view.RPC("squirt", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                }
            }
            else if (SceneManager.GetActiveScene().name == "Sloth")
            {
                if (GetComponent<PlayerUserTest>().gotBearTrapped && GetComponent<PlayerUserTest>().interactedBearTrap != null)
                {
                    view.RPC("trappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                    view.RPC("untrappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                }
                else if (GetComponent<PlayerUserTest>().alreadySetBearTrap != null && !GetComponent<PlayerUserTest>().gotBearTrapped)
                {
                    if (GetComponent<PlayerUserTest>().alreadySetBearTrap.GetComponent<SlothObstacle>().trapSet)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            view.RPC("untrappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                        }
                        else
                        {
                            view.RPC("untrappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                        }
                    }
                }
            }
            else if (SceneManager.GetActiveScene().name == "Wrath")
            {
            }
            else if (SceneManager.GetActiveScene().name == "Gluttony")
            {
            }
            else if (SceneManager.GetActiveScene().name == "Lust")
            {
            }
        }
    }


    [PunRPC]
    void walkies(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("isWalking", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void pushies(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("PushTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void hitties(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("AttackTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void draggies(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("isDragging", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // envy

    [PunRPC]
    void squirt(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("SquirtTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    // greed

    [PunRPC]
    void chipWalkies(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("isChipWalking", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void chipThrowies(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("ChipThrowTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // sloth
    [PunRPC]
    void trappies(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("isTrapped", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void untrappies(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("OpenTrapTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
