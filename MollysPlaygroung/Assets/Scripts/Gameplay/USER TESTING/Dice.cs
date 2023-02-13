using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public Rigidbody rb;

    bool hasLanded;
    bool thrown;

    Vector3 initialPos;
    Quaternion initialRot;

    public int rolledValue;

    public List<GameObject> diceSides = new List<GameObject>();

    public GameObject refPoint;

    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RollDice();
        }

        if (rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            DiceSideCheck();
        }
    }

    public void RollDice()
    {
        if(!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(100, 500), Random.Range(100, 500), Random.Range(100, 500));
            throwParabola(refPoint.transform.position, initialPos);
        }
        else if (thrown && hasLanded)
        {
            ResetDice();
        }
    }

    public void ResetDice()
    {
        rolledValue = 0;
        transform.position = initialPos;
        transform.rotation = initialRot;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    void DiceSideCheck()
    {
        for(int i = 0; i < diceSides.Count; i++)
        {
            if (diceSides[i].GetComponent<DiceSides>().onGround)
            {
                rolledValue = diceSides[i].GetComponent<DiceSides>().sideValue;
                Debug.Log("Dice rolled " + rolledValue);
                //GameObject.Find("GameManager").GetComponent<GreedGameManager>().DiceValueUpdate(rolledValue);
                //GameObject.Find("GameManager").GetComponent<GreedGameManager>().diceProcedure = false;
                ResetDice();
            }
        }
    }

    void MoveBoy(Vector3 vel)
    {
        rb.velocity = vel;
    }

    void throwParabola(Vector3 targetPos, Vector3 startPos)
    {
        Vector3 direction = targetPos - startPos;

        float height = direction.y;
        Vector3 halfRange = new Vector3(0, 0, direction.z);
        float Vy = Mathf.Sqrt(-2 * Physics2D.gravity.y * height);
        Vector3 VX = -(halfRange * Physics2D.gravity.y) / Vy;

        float randForceX = Random.Range(1, 3);
        float randForceY = Random.Range(0.5f, 1f);
        MoveBoy(new Vector3(0, Vy * randForceY, VX.z / randForceX));
    }
}
