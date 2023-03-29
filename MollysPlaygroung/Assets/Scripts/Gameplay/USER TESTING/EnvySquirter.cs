using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvySquirter : MonoBehaviour
{
    public bool squirterActivated = false;
    public float squirtTime;

    public string correlatingHorse;


    void FixedUpdate()
    {
        // Squirting water
        if (squirterActivated)
        {
            float newScale = Mathf.Lerp(transform.localScale.y, 0.005f, Time.deltaTime / squirtTime);
            transform.localScale = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
        }
        else
        {
            float newScale = Mathf.Lerp(transform.localScale.y, 0f, Time.deltaTime / squirtTime);
            transform.localScale = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
        }
    }
}
