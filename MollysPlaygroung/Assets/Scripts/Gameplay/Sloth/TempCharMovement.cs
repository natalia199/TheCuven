using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCharMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveLeftRight();
        moveForwardBack();
    }

    void moveLeftRight()
    {
        Vector3 left = Vector3.zero;
        left.x = Input.GetAxis("Horizontal");
        Vector3 v = new Vector3(left.x, 0f, 0f) * Time.deltaTime * 2.0f;
        this.transform.Translate(v, Space.Self);
    }

    void moveForwardBack()
    {
        Vector3 forward = Vector3.zero;
        forward.z = Input.GetAxis("Vertical");
        Vector3 v = new Vector3(0f, 0f, forward.z) * Time.deltaTime * 2.0f;
        this.transform.Translate(v, Space.Self);
    }

    /*
    void moveRotate()
    {
        Vector3 rotate = Vector3.zero;
        rotate.y = Input.GetAxis("Rotate");
        Vector3 v = new Vector3(0f, rotate.y, 0f) * Time.deltaTime * 2.0f;
        this.transform.Translate(v, Space.Self);
    }
    */
}
