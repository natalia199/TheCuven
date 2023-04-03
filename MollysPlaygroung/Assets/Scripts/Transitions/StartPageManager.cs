using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class StartPageManager : MonoBehaviourPunCallbacks
{
    public float minimum = 0.0f;
    public float maximum = 1f;
    public float duration = 5.0f;
    private float startTime;
    public Image sprite;

    public AudioSource titleMusic;

    bool oneTime;

    void Start()
    {
        startTime = Time.time;
        oneTime = false;
    }
    void Update()
    {
        if (!oneTime)
        {
            float t = (Time.time - startTime) / duration;
            sprite.color = new Color(0f, 0f, 0f, Mathf.SmoothStep(minimum, maximum, t));

            if (sprite.color.a == 0)
            {
                Destroy(sprite);
                oneTime = true;
            }
        }

    }
    public void ToUsername()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Failed to conntect to Photon " + cause.ToString(), this);
    }
}
