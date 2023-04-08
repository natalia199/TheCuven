using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonysFood : MonoBehaviour
{
    public MeshCollider colliderBoy;
    public bool newShmeat = false;


    void Awake()
    {
        if (newShmeat)
        {
            colliderBoy.enabled = true;
        }
        else
        {
            StartCoroutine("holdForFood");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            transform.tag = "Food";
        }
    }

    IEnumerator holdForFood()
    {
        yield return new WaitForSeconds(1f);

        colliderBoy.enabled = true;

    }
}
