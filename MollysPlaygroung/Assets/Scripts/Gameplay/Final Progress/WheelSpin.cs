using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour
{
    public float _rotationSpeed;
    public const float _maxRotationSpeed = 75f;
    public float _rotationSpeedDecrease = 200f;

    public bool _rotate;

    public float speedRemaining;

    public GameObject wheelDecision;

    public bool dontStartYet = true;

    public float maxSpeed = 1000;
    public float increaseRate = 400f;
    public float currentSpeed = 900;
    public bool increasing = true;

    public bool spinningHasFinished = false;

    void Start()
    {
        currentSpeed = 0;
        increasing = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<ResultGameManager>().SpinTheWheel)
        {
            if (increasing)
            {
                // increase or decrease the current speed depending on the value of increasing
                currentSpeed = Mathf.Clamp(currentSpeed + Time.deltaTime * increaseRate * (increasing ? 1 : -1), 0, maxSpeed);
                transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);

                if (currentSpeed >= 900)
                {
                    GameObject.Find("GameManager").GetComponent<ResultGameManager>().accessToSpin = true;
                }
            }
            else
            {

                if (currentSpeed > 0)
                {
                    if (currentSpeed <= 400 && currentSpeed > 100)
                    {
                        increaseRate = 100;
                    }
                    else if (currentSpeed <= 100 && currentSpeed > 40)
                    {
                        increaseRate = 40;
                    }
                    else if (currentSpeed <= 40 && currentSpeed > 10)
                    {
                        increaseRate = 10;
                    }
                    else if (currentSpeed <= 10 && currentSpeed > 0)
                    {
                        increaseRate = 3;
                    }

                    currentSpeed = Mathf.Clamp(currentSpeed + Time.deltaTime * increaseRate * (increasing ? 1 : -1), 0, maxSpeed);
                    transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
                }
                else
                {
                    spinningHasFinished = true;

                }
            }
        }
    }
}
