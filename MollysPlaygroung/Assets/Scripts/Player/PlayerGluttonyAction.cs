using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerGluttonyAction : MonoBehaviour
{
    PhotonView view;

    //public TextMeshProUGUI username;
    //string playersUsername;

    private bool btnPressed = false;

    bool oneTime = false;
    bool gameDone = false;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gluttony")
        {
            if (!oneTime)
            {
                Physics2D.IgnoreLayerCollision(6, 6, true);
                oneTime = true;
            }

            // Player actions
            if (view.IsMine && !gameDone)
            {
                this.transform.GetChild(0).gameObject.SetActive(true);
                this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, this.GetComponent<SpriteRenderer>().color.g, this.GetComponent<SpriteRenderer>().color.b, 1f);

                view.RPC("setUsername", RpcTarget.AllBuffered, view.Owner.NickName);

                if (Input.GetKeyDown(KeyCode.X) && btnPressed)
                {
                    view.RPC("gluttonyCompletion", RpcTarget.AllBuffered);
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
    }

    [PunRPC]
    void setUsername(string Player)
    {
        try
        {
            GameObject.Find(Player).transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find(Player).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Player;
            //GameObject.Find(Player).GetComponent<PlayerGluttonyAction>().username.text = Player;
            GameObject.Find(Player).GetComponent<SpriteRenderer>().color = new Color(GameObject.Find(Player).GetComponent<SpriteRenderer>().color.r, GameObject.Find(Player).GetComponent<SpriteRenderer>().color.g, GameObject.Find(Player).GetComponent<SpriteRenderer>().color.b, 1f);
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
    void gluttonyCompletion()
    {        
        GameObject.Find(view.Owner.NickName).GetComponent<PlayerGluttonyAction>().gameDone = true;

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("Transition");
    }

    void OnTriggerStay2D(Collider2D col)
    {
        try
        {
            if (view.IsMine)
            {
                Debug.Log("Colliding");
                if (col.tag == "Button")
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
                if (col.tag == "Button")
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
