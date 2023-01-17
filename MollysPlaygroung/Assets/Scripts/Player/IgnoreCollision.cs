using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField] Transform[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        Collider selfCollider = GetComponent<Collider>();
        //Not sure how to convert this over to networkable code
        foreach(Transform col in colliders)
        {
            Physics.IgnoreCollision(col.GetComponent<Collider>(), selfCollider, true);
        }
        
    }
}
