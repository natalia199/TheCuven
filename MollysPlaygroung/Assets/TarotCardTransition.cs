using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardTransition : MonoBehaviour
{
    public Vector3 moveHandTo;

    public GameObject hand;

    public bool moveHandNow = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("moveHand");
    }


    // Update is called once per frame
    void Update()
    {
        if (moveHandNow)
        {
            hand.transform.position = Vector3.Lerp(hand.transform.position, moveHandTo, Time.deltaTime * 3f);
        }
    }

    IEnumerator moveHand()
    {
        yield return new WaitForSeconds(3);

        moveHandNow = true;
    }
}
