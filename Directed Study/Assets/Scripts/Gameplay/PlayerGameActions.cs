using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class PlayerGameActions : MonoBehaviour
{
    PhotonView view;

    public TextMeshProUGUI username;
    //public Canvas playerCanvas;
    string playersUsername;

    public bool leverToggle;
    private bool btnPressed = false;

    bool settingUsername = false;

    void Start()
    {
        view = GetComponent<PhotonView>();
        Physics2D.IgnoreLayerCollision(6, 6, true);

        if (view.IsMine)
        {
            leverToggle = false;
        }
    }

    void Update()
    {
        // Naming player's gameobject corresponding actor number
        if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }
        this.name = playersUsername;

        if (view.IsMine)
        {
            view.RPC("setUsername", RpcTarget.AllBuffered, view.Owner.NickName);

            if (Input.GetKeyDown(KeyCode.X) && btnPressed)
            {
                if(leverToggle)
                {
                    view.RPC("switchLever", RpcTarget.AllBuffered, true);
                    view.RPC("colourChange", RpcTarget.AllBuffered, 255f, 0f, 0f);
                    leverToggle = false;
                }
                else
                {
                    view.RPC("switchLever", RpcTarget.AllBuffered, false);
                    view.RPC("colourChange", RpcTarget.AllBuffered, 0f, 0f, 255f);
                    leverToggle = true;
                }
            }


            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.Translate(Vector2.down * -Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.Translate(Vector2.down * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                view.RPC("flipPlayer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName, true);
                this.transform.Translate(Vector2.left * -Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                view.RPC("flipPlayer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName, false);
                this.transform.Translate(Vector2.left * Time.deltaTime);
            }
        }
    }

    // Sharing owner's player number with others
    [PunRPC]
    void getPlayersNickName(string name)
    {
        playersUsername = name;
    }

    [PunRPC]
    void setUsername(string Player)
    {
        try
        {
            GameObject.Find(Player).GetComponent<PlayerGameActions>().username.text = Player;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void flipPlayer(string name, bool flip)
    {
        try
        {
            GameObject.Find(name).GetComponent<SpriteRenderer>().flipX = flip;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void colourChange(float r, float g, float b)
    {
        GameObject.Find("Square").GetComponent<SpriteRenderer>().color = new Color(r,g,b);
    }

    [PunRPC]
    void switchLever(bool flip)
    {
        try
        {
            if (!flip)
            {
                GameObject.Find("LeverTop").GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 25f);
            }
            else
            {
                GameObject.Find("LeverTop").GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -25f);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        try
        {
            if (view.IsMine)
            {
                if (col.tag == "Door")
                {
                    Debug.Log("Spawning to " + col.name);
                    this.gameObject.transform.position = GameObject.Find(col.name).GetComponent<RoomTransition>().changeRoomView().position;
                }
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        try
        {
            if (view.IsMine)
            {
                if (col.tag == "Lever")
                {
                    btnPressed = true;
                }
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        try
        {
            if (view.IsMine)
            {
                if (col.tag == "Lever")
                {
                    btnPressed = false;
                }
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }

    }
}
