using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShakeWrath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scalingFactor = 1; // Bigger for slower
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime / scalingFactor);
    }
}
