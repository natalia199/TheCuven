using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGreed : MonoBehaviour
{
    bool carryChip;
    bool withinZone;

    Rigidbody rb;
    public float moveSpeed;

    Vector3 keyboardMovement;

    GameObject currentChip;
    GameObject currentZone;

    public List<GameObject> carriedChips = new List<GameObject>();

    public int throwCount;
    public int carryCount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        carryChip = false;
        withinZone = true;
        throwCount = 0;
    }

    void Update()
    {
        keyboardMovement.x = Input.GetAxisRaw("Horizontal");
        keyboardMovement.z = Input.GetAxisRaw("Vertical");

        if (!GameObject.Find("GameManager").GetComponent<GreedGameManager>().diceProcedure)
        {
            if (throwCount >= GameObject.Find("GameManager").GetComponent<GreedGameManager>().diceValueRolled)
            {
                carryCount = 0;
                throwCount = 0;
                GameObject.Find("GameManager").GetComponent<GreedGameManager>().diceProcedure = true;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (carryChip && currentChip != null && carryCount < GameObject.Find("GameManager").GetComponent<GreedGameManager>().diceValueRolled)
                {
                    if (currentChip.GetComponent<ChipScript>().Available)
                    {
                        if (carriedChips.Count == 0)
                        {
                            GameObject.Find("GameManager").GetComponent<GreedGameManager>().FirstCarriedChip(currentChip, this.gameObject, carriedChips.Count);
                        }
                        else
                        {
                            GameObject.Find("GameManager").GetComponent<GreedGameManager>().CarryMoreChips(currentChip, this.gameObject, carriedChips[carriedChips.Count - 1], carriedChips.Count);
                        }

                        carriedChips.Add(currentChip);
                        currentChip = null;

                        carryCount++;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (carriedChips.Count > 0 && withinZone)
                {
                    GameObject.Find("GameManager").GetComponent<GreedGameManager>().DropTheChip(carriedChips[carriedChips.Count - 1], this.gameObject, currentZone);

                    carriedChips.RemoveAt(carriedChips.Count - 1);
                    throwCount++;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameObject.Find("GameManager").GetComponent<GreedGameManager>().RollingTheDice();
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + keyboardMovement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChipZone")
        {
            withinZone = true;
            currentZone = other.gameObject;
        }

        if (other.tag == "Chip" && currentChip == null)
        {
            carryChip = true;
            currentChip = other.transform.parent.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "ChipZone")
        {
            withinZone = false;
            currentZone = null;
        }

        if (other.tag == "Chip")
        {
            carryChip = false;
            currentChip = null;
        }
    }
}
