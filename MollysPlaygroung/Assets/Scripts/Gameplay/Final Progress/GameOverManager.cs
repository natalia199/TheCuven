using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviourPunCallbacks
{
    public GameObject skipBtn;
    public GameObject theCredits;

    public bool startCredits = false;

    public float m_Speed;

    void Start()
    {
        StartCoroutine("CreditsFinished");
    }

    public void FixedUpdate()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            skipBtn.SetActive(true);
        else
            skipBtn.SetActive(false);

        if (startCredits && theCredits.transform.position.y < 2600f)
        {
            theCredits.GetComponent<Rigidbody>().velocity = transform.up * m_Speed;
        }
    }


    IEnumerator CreditsFinished()
    {
        yield return new WaitForSeconds(5);

        startCredits = true;
    }
}
