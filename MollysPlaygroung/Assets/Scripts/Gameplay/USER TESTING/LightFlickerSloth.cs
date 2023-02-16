using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerSloth : MonoBehaviour
{
    public GameObject light;

    public float MinTime;
    public float MaxTime;
    public float Timer;

    bool onState = false;
    public bool flickerLifeLength = true;

    public int step;

    void Start()
    {
        Timer = Random.Range(MinTime, MaxTime);

        StartCoroutine("FlickerTime", Random.Range(2, 3));
    }

    // Update is called once per frame
    void Update()
    {
        if (flickerLifeLength)
        {
            FlickerIn();
        }
        else
        {
            light.SetActive(true);
        }
    }

    public void FlickerIn()
    {
        if(Timer > 0)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer <= 0)
        {
            light.SetActive(onState);
            Timer = Random.Range(MinTime, MaxTime);
            onState = !onState;
        }
    }
    IEnumerator FlickerTime(float time)
    {
        yield return new WaitForSeconds(time);

        flickerLifeLength = false;
    }

    public void LightsOut()
    {
        StartCoroutine("DieOut", Random.Range(2, 4));
    }

    IEnumerator DieOut(float time)
    {
        // added step, test it out by adding a vlue in inpector and seeing if collider moves downward
        transform.GetChild(0).GetComponent<CapsuleCollider>().center = Vector3.MoveTowards(transform.GetChild(0).GetComponent<CapsuleCollider>().center, new Vector3(transform.GetChild(0).GetComponent<CapsuleCollider>().center.x, transform.GetChild(0).GetComponent<CapsuleCollider>().center.y - 20f, transform.GetChild(0).GetComponent<CapsuleCollider>().center.z), step);

        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }
}
