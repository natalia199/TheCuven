using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInstantiation : MonoBehaviour
{
    public GameObject lightSource;

    public Vector2 leftFrontRange;
    public Vector2 backRightRange;

    float timeLeft = 0.0f;

    float timeDifference = 3.0f;

    void Start()
    {
        Instantiate(lightSource, new Vector3(Random.Range(leftFrontRange.x, backRightRange.x), 0f, Random.Range(leftFrontRange.y, backRightRange.y)), Quaternion.identity);
    }

    void Update()
    {
        timeLeft += Time.deltaTime;
        if (timeLeft > timeDifference)
        {

        }
    }

    void instantiateLight()
    {
        // every X amount of time, instantiate a new light with a random placement
        timeDifference += Random.Range(2.0f, 10.0f);
    }
}