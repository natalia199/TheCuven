using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    int lifeValue;

    public bool outOfLight;
    bool decreaseHer;
    int timeBackUp;

    bool obstacleCollision;
    GameObject currentObstacle;

    public Rigidbody rb;
    public float moveSpeed;

    Vector3 keyboardMovement;

    void Start()
    {
        lifeValue = 100;
        outOfLight = true;
        decreaseHer = true;
        obstacleCollision = false;
        timeBackUp = 1;
    }

    void Update()
    {
        keyboardMovement.x = Input.GetAxisRaw("Horizontal");
        keyboardMovement.z = Input.GetAxisRaw("Vertical");

        if (outOfLight)
        {
            if(decreaseHer)
            {
                StartCoroutine("LifeDrop", 0.5f);
            }
        }
        //RPC
        GameObject.Find("GameManager").GetComponent<SlothGameManager>().DisplayerLifeBar(lifeValue);

        if (Input.GetKeyDown(KeyCode.X))
        {
            /// For mutliplayer you want it to be
            /// if (!obstacleCollision && currentObstacle.GetComponent<SlothObstacle>().trapSet) then deactivate the trap ur colliding with
            
            if (obstacleCollision && currentObstacle.GetComponent<SlothObstacle>().trapSet)
            {
                obstacleCollision = false;
                currentObstacle.GetComponent<SlothObstacle>().DisintegrateObstacle();
            }
        }
    }

    void FixedUpdate()
    {
        if (!obstacleCollision)
        {
            rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SlothObstacle")
        {
            currentObstacle = other.transform.parent.gameObject;
            /// For non-multiplayer maybe add a stun effect after they get caught so prevent any btn pressing for 3s

            // RPC
            // if trap ISNT set yet
            obstacleCollision = true;
            GameObject.Find("GameManager").GetComponent<SlothGameManager>().ActivateTrap(other.transform.parent.gameObject, this.gameObject);
            // if trap IS set then call the "DeactivateTrap()" and leave obstacleCollision as False
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Light")
        {
            outOfLight = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Light")
        {
            decreaseHer = true;
            outOfLight = true;
        }

        if (other.tag == "SlothObstacle")
        {
            currentObstacle = null;
            obstacleCollision = false;
        }
    }

    IEnumerator LifeDrop(int value)
    {
        lifeValue -= timeBackUp;
        decreaseHer = false;

        yield return new WaitForSeconds(value);

        decreaseHer = true;
    }
}
