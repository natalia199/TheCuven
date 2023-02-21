using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathGameplayManager : MonoBehaviour
{
    public int directionIndex;
    public bool returnHome;

    public GameObject lava;
    public float rotationsPerMinute;

    public GameObject wrathPlate;

    public bool plateState = false;


    void Start()
    {
        plateState = wrathPlate.GetComponent<PlatformShakeWrath>().newTilt;

    }

    void FixedUpdate()
    {
        lava.transform.Rotate(0, 6 * rotationsPerMinute * Time.deltaTime, 0);

        plateState = wrathPlate.GetComponent<PlatformShakeWrath>().newTilt;
    }

    public void shakePlateVariables(int dir)
    {
        wrathPlate.GetComponent<PlatformShakeWrath>().newTilt = false;
        plateState = false;
        wrathPlate.GetComponent<PlatformShakeWrath>().setVars(dir);
    }
}
