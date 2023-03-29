using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlothObstacle : MonoBehaviour
{
    public bool trapSet;

    public GameObject caughtPlayer;

    void Start()
    {
        trapSet = false;
    }

    public void selfDestruct()
    {
        StartCoroutine("pauseDeath", 1f);
    }

    IEnumerator pauseDeath(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        
        // Bear trap
        if (other.tag == "Player" && !trapSet)
        {
            caughtPlayer = other.gameObject;
            trapSet = true;
            this.tag = "SetBearTrap";
        }
    }
    void OnTriggerExit(Collider other)
    {

    }
}
