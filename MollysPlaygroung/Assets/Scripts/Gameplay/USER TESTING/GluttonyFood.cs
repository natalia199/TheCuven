using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyFood : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void EatMe()
    {
        Destroy(this.gameObject);
    }

    // Change for RPC - player destroys obj when it runs into it
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EatMe();
        }
    }
}
