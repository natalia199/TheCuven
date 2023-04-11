using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviourPunCallbacks
{
    public GameObject skipBtn;
    public GameObject theCredits;

    public bool startCredits = false;
    public bool opacityChange = false;

    public float m_Speed;

    public TextMeshProUGUI w;
    public TextMeshProUGUI inner;
    public TextMeshProUGUI s;

    public SpriteRenderer fadeImg;

    void Awake()
    {
        //GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState = "game over";
    }

    void Start()
    {
        StartCoroutine("CreditsFinished");
    }

    public void FixedUpdate()
    {
        /*
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            skipBtn.SetActive(true);
        else
            skipBtn.SetActive(false);
        */
        if (startCredits && theCredits.transform.position.y < 2600f)
        {
            theCredits.GetComponent<Rigidbody>().velocity = transform.up * m_Speed;
        }
    }


    IEnumerator CreditsFinished()
    {
        yield return new WaitForSeconds(8);

        startCredits = true;
    }

    public void SkipCredits()
    {
        startCredits = false;

        StartCoroutine("OpacityChangePause");

        theCredits.SetActive(false);
    }

    IEnumerator OpacityChangePause()
    { 
        StartCoroutine("FadeTo");
        StartCoroutine("FadeImg");

        yield return new WaitForSeconds(3f);

        StartCoroutine("FadeIn");

        yield return new WaitForSeconds(4f);

        StartCoroutine("FadeOut");
    }

    IEnumerator FadeTo()
    {
        w.color = new Color(w.color.r, w.color.g, w.color.b, 1);
        while (w.color.a > 0.0f)
        {
            w.color = new Color(w.color.r, w.color.g, w.color.b, w.color.a - (Time.deltaTime / 3f));
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        s.color = new Color(s.color.r, s.color.g, s.color.b, 0);
        while (s.color.a < 1.0f)
        {
            s.color = new Color(s.color.r, s.color.g, s.color.b, s.color.a + (Time.deltaTime /3f));
            yield return null;
        }
    }
    
    IEnumerator FadeImg()
    {
        fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 0);
        while (fadeImg.color.a < 1.0f)
        {
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, fadeImg.color.a + (Time.deltaTime / 4f));
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        inner.color = new Color(inner.color.r, inner.color.g, inner.color.b, 1);
        while (inner.color.a > 0.0f)
        {
            s.color = new Color(s.color.r, s.color.g, s.color.b, s.color.a - (Time.deltaTime / 3f));
            inner.color = new Color(inner.color.r, inner.color.g, inner.color.b, inner.color.a - (Time.deltaTime / 3f));
            yield return null;
        }

        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Loading");
    }
}
