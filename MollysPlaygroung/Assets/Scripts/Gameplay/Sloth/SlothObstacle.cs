using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlothObstacle : MonoBehaviour
{
    public bool trapSet;

    void Start()
    {
        trapSet = false;
    }

    void Update()
    {
        
    }

    public void InitiateTrap()
    {
        trapSet = true;
    }

    public void DisintegrateObstacle()
    {
        Destroy(this.gameObject);
    }
}
