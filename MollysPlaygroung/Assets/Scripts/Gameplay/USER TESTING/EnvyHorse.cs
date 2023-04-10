using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvyHorse : MonoBehaviour
{
    public Transform finishLinePoint;
    public Vector3 startingLinePoint;

    public bool finished;
    public bool squirterActivated;
    public float thpeed;

    public float speed;
    public float RotAngleY;

    public int id;

    public bool resetingVars = false;

    void Start()
    {
        finished = false;

        startingLinePoint = this.transform.position;

        // Setting gun name based on ID
        //GameObject.Find("SquirtGun" + id).transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!finished && squirterActivated)
        {
            float rY = Mathf.SmoothStep(-RotAngleY, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
            transform.rotation = Quaternion.Euler(0, 0, rY);
        }
        else if (finished)
        {
            float rY = Mathf.SmoothStep(-RotAngleY, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
            transform.rotation = Quaternion.Euler(0, 0, rY);
        }
        else
        {
            float rY = Mathf.SmoothStep(0, 0, Mathf.PingPong(Time.time * speed, 1));
            transform.rotation = Quaternion.Euler(0, 0, rY);
        }

        if (resetingVars)
        {
            resetAll();
        }
    }

    public void MoveYourHorse()
    {
        Debug.Log("moving");
        squirterActivated = true;
    }

    public void StopYourHorse()
    {
        Debug.Log("Stopping");
        squirterActivated = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            finished = true;
            GameObject.Find("SquirtGun" + id).transform.GetChild(2).GetChild(0).gameObject.SetActive(false);                    // turning off gun light
            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().RecordHorseResult(this.gameObject);              // adding winning horse to result list
        }
    }

    public void resetAll()
    {        
        this.transform.position = startingLinePoint;

        GameObject.Find("SquirtGun" + id).transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        squirterActivated = false;
        finished = false;
    }
}
