using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip gettingSmacked;
    public AudioClip smacking;
    public AudioClip jumping;
    public AudioClip gettingPushed;

    public float volume = 1;

    public void gettingSmackedSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(gettingSmacked, GameObject.Find(pname).transform.position, volume);
    }

    public void smackingSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(smacking, GameObject.Find(pname).transform.position, volume);
    }

    public void gettingPushedSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(gettingPushed, GameObject.Find(pname).transform.position, volume);
    }

    public void jumpingSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(jumping, GameObject.Find(pname).transform.position, volume);
    }
}
