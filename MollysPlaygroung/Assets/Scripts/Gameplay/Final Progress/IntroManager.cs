using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GameObject skipBtn;

    void Start()
    {
        StartCoroutine("AnimationFinished");
    }

    public void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            skipBtn.SetActive(true);
        else
            skipBtn.SetActive(false);
    }


    public void ToTransitionScene()
    {
        PhotonNetwork.LoadLevel("Intro_transition_Card");
    }

    IEnumerator AnimationFinished()
    {
        yield return new WaitForSeconds(65f);

        ToTransitionScene();
    }
}
