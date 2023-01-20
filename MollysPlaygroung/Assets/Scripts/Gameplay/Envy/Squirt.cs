using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirt : MonoBehaviour
{
    public float maxScale = 10f;
    public float speed = 1f;

    private Vector3 v3OrgPos;
    private float orgScale;
    private float endScale;

    void Start()
    {
        v3OrgPos = transform.position - transform.forward;
        orgScale = transform.localScale.z;
        endScale = orgScale;
    }

    // Update is called once per frame
    void Update()
    {
        ResizeOn();
    }

    private void ResizeOn()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.MoveTowards(transform.localScale.z, endScale, Time.deltaTime * speed));
        transform.position = v3OrgPos + (transform.forward) * (transform.localScale.z / 2.0f + orgScale / 2.0f);
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("yeur");
            endScale = maxScale;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("naur");
            endScale = orgScale;
        }
    }
}
