using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerSlothActions : MonoBehaviour
{
    // Start is called before the first frame update

    PhotonView view;
    bool gameDone =false;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( SceneManager.GetActiveScene().name == "Sloth")
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, this.GetComponent<SpriteRenderer>().color.g, this.GetComponent<SpriteRenderer>().color.b, 1f);


            if (view.IsMine && !gameDone)
            {
                //NAT IS A FUCKING MANIAC LET IT BE KNOWN
                Debug.Log("This is a test");
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
                    this.transform.Translate(Vector2.right * -Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    this.transform.Translate(Vector2.left * Time.deltaTime);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    view.RPC("slothCompletion", RpcTarget.AllBuffered);
                }
            }
        }
    }
    [PunRPC]
    void slothCompletion()
    {
        GameObject.Find(view.Owner.NickName).GetComponent<PlayerSlothActions>().gameDone = true;

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("Transition");
    }
}
