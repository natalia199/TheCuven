using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvySquirter : MonoBehaviour
{
    public bool squirterActivated = false;
    public float squirtTime;

    public string correlatingHorse;

    public GameObject warpedWater;

    void FixedUpdate()
    {
        // Squirting water
        if (squirterActivated)
        {
            //float newScale = Mathf.Lerp(transform.localScale.y, 0.005f, Time.deltaTime / squirtTime);
            float newScale = Mathf.Lerp(warpedWater.transform.localScale.z, 0.05f, Time.deltaTime / squirtTime);
            //transform.localScale = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
            warpedWater.transform.localScale = new Vector3(warpedWater.transform.localScale.x, warpedWater.transform.localScale.y, newScale);
        }
        else
        {
            //float newScale = Mathf.Lerp(transform.localScale.y, 0f, Time.deltaTime / squirtTime);
            float newScale = Mathf.Lerp(warpedWater.transform.localScale.z, 0f, Time.deltaTime / squirtTime);
            //transform.localScale = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
            warpedWater.transform.localScale = new Vector3(warpedWater.transform.localScale.x, transform.localScale.y, newScale);
        }
    }
}
