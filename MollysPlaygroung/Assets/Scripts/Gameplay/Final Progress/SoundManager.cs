using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip gettingSmacked;
    public AudioClip smacking;
    public AudioClip jumping;
    public AudioClip gettingPushed;
    /*
    public AudioClip bearTrapClose;
    public AudioClip bearTrapDrop;
    public AudioClip bearTrapOpen;
    public AudioClip chipDrop;
    public AudioClip chipStack;
    public AudioClip diceRoll;
    public AudioClip dizzy;
    public AudioClip falling;
    public AudioClip lightFlicker;
    public AudioClip meatSlap;
    public AudioClip rustyCapybara;
    public AudioClip waterDrink;
    public AudioClip waterPour;
    
    public AudioClip munching;
    public AudioClip poisoned;
*/
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

    /*

    public void bearTrapCloseSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(bearTrapClose, GameObject.Find(pname).transform.position, volume);
    }

    public void bearTrapDropSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(bearTrapDrop, GameObject.Find(pname).transform.position, volume);
    }

    public void bearTrapOpenSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(bearTrapOpen, GameObject.Find(pname).transform.position, volume);
    }

    public void chipDropSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(chipDrop, GameObject.Find(pname).transform.position, volume);
    }

    public void chipStackSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(chipStack, GameObject.Find(pname).transform.position, volume);
    }

    public void diceRollSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(diceRoll, GameObject.Find(pname).transform.position, volume);
    }

    public void dizzySFX(string pname)
    {
        AudioSource.PlayClipAtPoint(dizzy, GameObject.Find(pname).transform.position, volume);
    }

    public void fallingSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(falling, GameObject.Find(pname).transform.position, volume);
    }

    public void lightFlickerSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(lightFlicker, GameObject.Find(pname).transform.position, volume);
    }

    public void meatSlapSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(meatSlap, GameObject.Find(pname).transform.position, volume);
    }

    public void rustyCapybaraSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(rustyCapybara, GameObject.Find(pname).transform.position, volume);
    }

    public void waterDrinkSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(waterDrink, GameObject.Find(pname).transform.position, volume);
    }

    public void waterPourSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(waterPour, GameObject.Find(pname).transform.position, volume);
    }     

    public void poisonedSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(poisoned, GameObject.Find(pname).transform.position, volume);
    }

    public void munchingSFX(string pname)
    {
        AudioSource.PlayClipAtPoint(munching, GameObject.Find(pname).transform.position, volume);
    }  */
}
