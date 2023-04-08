using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbominationsAnimations : MonoBehaviour
{
    PhotonView view;

    int characterNum = -1;

    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            characterNum = getCharacterNumber();
        }
    }

    void Update()
    {
        if (view.IsMine && GameObject.Find("Scene Manager").GetComponent<SceneManage>().countdownLevelCheck)
        {
            if (getCharacterNumber() > -1)
            {
                if (SceneManager.GetActiveScene().name != "LevelResult")
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "rumble" || GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "game over")
                    {
                        if (GetComponent<PlayerUserTest>().playerIsGrounded) 
                        {
                            if (GetComponent<PlayerUserTest>().freezePlayer)
                            {
                                view.RPC("ouchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, GetComponent<PlayerUserTest>().oopsyGotHit, characterNum);

                                view.RPC("aghhh", RpcTarget.AllBufferedViaServer, view.Owner.NickName, GetComponent<PlayerUserTest>().oopsyGotDragged, characterNum);
                            }
                            else
                            {
                                view.RPC("ouchies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                view.RPC("aghhh", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);

                                if (Input.GetKeyDown(KeyCode.Space))
                                {
                                    view.RPC("jumpies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                }

                                if (!GetComponent<PlayerUserTest>().actionPause)
                                {
                                    if (Input.GetKeyDown(KeyCode.O))
                                    {
                                        view.RPC("pushies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                    }
                                }

                                if (Input.GetKeyDown(KeyCode.I))
                                {
                                    view.RPC("hitties", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                }

                                if (GetComponent<PlayerUserTest>().oopsyGotPushed)
                                {
                                    GetComponent<PlayerUserTest>().oopsyGotPushed = false;
                                    view.RPC("oof", RpcTarget.AllBufferedViaServer, view.Owner.NickName, characterNum);
                                }

                                if (Input.GetKey(KeyCode.P))
                                {
                                    view.RPC("draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                }
                                else
                                {
                                    view.RPC("draggies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    int getCharacterNumber()
    {
        for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username == PhotonNetwork.LocalPlayer.NickName)
            {
                return GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID;
            }
        }

        return -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine && GameObject.Find("Scene Manager").GetComponent<SceneManage>().countdownLevelCheck)            
        {
            characterNum = getCharacterNumber();
            Debug.Log("num " + characterNum);

            if (getCharacterNumber() > -1)
            {
                bool isWalking = transform.GetChild(2).GetChild(characterNum).GetComponent<Animator>().GetBool("isWalking");

                float inputHorizontal = Input.GetAxisRaw("Horizontal");
                float inputVertical = Input.GetAxisRaw("Vertical");
                bool directionPressed = inputHorizontal != 0 || inputVertical != 0;

                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "rumble" || GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "game over")
                {
                    if (SceneManager.GetActiveScene().name != "LevelResult")
                    {
                        if (GetComponent<PlayerUserTest>().playerIsGrounded)
                        {
                            if (!GetComponent<PlayerUserTest>().freezePlayer)
                            {
                                if (SceneManager.GetActiveScene().name == "Greed")
                                {
                                    // walking
                                    if (!isWalking && directionPressed)
                                    {

                                        if (GetComponent<PlayerUserTest>().collectedChipies.Count > 0) //holding shift toggles walk and chip walk
                                        {
                                            view.RPC("chipWalkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                        }
                                        else
                                        {
                                            view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                            view.RPC("chipWalkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                        }
                                    }

                                    if (isWalking && !directionPressed)
                                    {
                                        view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                    }
                                }
                                else
                                {
                                    // walking
                                    if (!isWalking && directionPressed)
                                    {

                                        view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);

                                    }

                                    if (isWalking && !directionPressed)
                                    {
                                        view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                    }
                                }
                            }
                            else
                            {
                                view.RPC("walkies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                            }


                            if (SceneManager.GetActiveScene().name == "Greed")
                            {

                                if (Input.GetKeyDown(KeyCode.R))
                                {
                                    // throw
                                    view.RPC("chipThrowies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                }
                                else
                                {
                                    view.RPC("chipThrowies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
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
                                            view.RPC("squirt", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                        }
                                    }
                                    else
                                    {
                                        view.RPC("squirt", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                    }
                                }
                                else
                                {
                                    view.RPC("squirt", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                }
                            }
                            else if (SceneManager.GetActiveScene().name == "Sloth")
                            {
                                if (GetComponent<PlayerUserTest>().gotBearTrapped && GetComponent<PlayerUserTest>().interactedBearTrap != null)
                                {
                                    view.RPC("trappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                    view.RPC("untrappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                }
                                else if (GetComponent<PlayerUserTest>().alreadySetBearTrap != null && !GetComponent<PlayerUserTest>().gotBearTrapped)
                                {
                                    if (GetComponent<PlayerUserTest>().alreadySetBearTrap.GetComponent<SlothObstacle>().trapSet)
                                    {
                                        if (Input.GetKeyDown(KeyCode.E))
                                        {
                                            view.RPC("untrappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                        }
                                        else
                                        {
                                            view.RPC("untrappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                        }
                                    }
                                }

                                if (!GetComponent<PlayerUserTest>().gotBearTrapped)
                                {
                                    view.RPC("trappies", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                }
                            }
                            else if (SceneManager.GetActiveScene().name == "Wrath")
                            {
                            }
                            else if (SceneManager.GetActiveScene().name == "Gluttony")
                            {
                                if (GetComponent<PlayerUserTest>().interactedOpponent != null && GetComponent<PlayerUserTest>().bigBoyMunch)
                                {
                                    if (Input.GetKeyDown(KeyCode.E))
                                    {
                                        view.RPC("bigBoyMunching", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                    }
                                    else
                                    {
                                        view.RPC("bigBoyMunching", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                    }
                                }
                                else
                                {
                                    view.RPC("bigBoyMunching", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false, characterNum);
                                }

                            }
                            else if (SceneManager.GetActiveScene().name == "Lust")
                            {
                            }
                            else if (SceneManager.GetActiveScene().name == "Pride")
                            {
                                if (GetComponent<PlayerUserTest>().gotPoisoned)
                                {
                                    view.RPC("poisonedLikeABitch", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true, characterNum);
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    [PunRPC]
    void walkies(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("isWalking", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void jumpies(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().jumpingSFX(name);

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("JumpTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void ouchies(string pName, bool x, int charNum)
    {
        try
        {
            if (x) 
            {
                GameObject.Find("SoundManager").GetComponent<SoundManager>().gettingSmackedSFX(name);

                GameObject.Find(pName).GetComponent<PlayerUserTest>().oopsyGotHit = false;
                GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("HitTrigger", true);
            }
            else
            {
                GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("HitTrigger", false);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void oof(string pName, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().gettingPushedSFX(name);

            GameObject.Find(pName).GetComponent<PlayerUserTest>().oopsyGotPushed = false;
            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("PushedTrigger", true);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void aghhh(string pName, bool x, int charNum)
    {
        try
        {
            if (x)
            {
                GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("isBeingDragged", true);
            }
            else
            {
                GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("isBeingDragged", false);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void pushies(string pName, bool x, int charNum)
    {
        try
        {

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("PushTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void hitties(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().smackingSFX(name);

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("AttackTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void draggies(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("isDragging", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // envy

    [PunRPC]
    void squirt(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().rustyCapybaraSFX(name);

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("SquirtTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    // greed

    [PunRPC]
    void chipWalkies(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("isChipWalking", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void chipThrowies(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().chipDropSFX(name);

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("ChipThrowTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // sloth
    [PunRPC]
    void trappies(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().bearTrapCloseSFX(name);

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("isTrapped", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void untrappies(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().bearTrapOpenSFX(name);

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("OpenTrapTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void bigBoyMunching(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().munchingSFX(name);

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("OpenTrapTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void poisonedLikeABitch(string pName, bool x, int charNum)
    {
        try
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().poisonedSFX(name);

            GameObject.Find(pName).transform.GetChild(2).GetChild(charNum).GetComponent<Animator>().SetBool("PoisonTrigger", x);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

}
