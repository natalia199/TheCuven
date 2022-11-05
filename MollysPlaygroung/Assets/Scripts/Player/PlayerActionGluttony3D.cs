using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerActionGluttony3D : MonoBehaviour
{
    PhotonView view;


    bool gameDone = false;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gluttony")
        {

            // Player actions
            if (view.IsMine && !gameDone)
            {
                this.transform.GetChild(0).gameObject.SetActive(true);

                view.RPC("setUsername", RpcTarget.AllBuffered, view.Owner.NickName);

                if (Input.GetKeyDown(KeyCode.X))
                {
                    view.RPC("gluttonyCompletion", RpcTarget.AllBuffered);

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
           // GameObject.Find(Player).GetComponent<SpriteRenderer>().color = new Color(GameObject.Find(Player).GetComponent<SpriteRenderer>().color.r, GameObject.Find(Player).GetComponent<SpriteRenderer>().color.g, GameObject.Find(Player).GetComponent<SpriteRenderer>().color.b, 1f);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void gluttonyCompletion()
    {
        GameObject.Find(view.Owner.NickName).GetComponent<PlayerActionGluttony3D>().gameDone = true;

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("Transition");
    }
}
