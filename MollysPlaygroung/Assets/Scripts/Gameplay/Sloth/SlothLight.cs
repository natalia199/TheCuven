using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlothLight : MonoBehaviour
{
    bool endFade;
    int scalingFramesLeft;

    void Start()
    {
        endFade = false;
        scalingFramesLeft = 0;
    }

    void FixedUpdate()
    {
        if (endFade)
        {
            if (scalingFramesLeft > 2f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 1, 0), Time.deltaTime * 3);

                if (scalingFramesLeft > 4f)
                {
                    transform.GetChild(0).GetComponent<CapsuleCollider>().center = new Vector3(transform.GetChild(0).GetComponent<CapsuleCollider>().center.x, 1f, transform.GetChild(0).GetComponent<CapsuleCollider>().center.z);
                    transform.GetChild(0).GetComponent<CapsuleCollider>().height = 0;
                }

                scalingFramesLeft--;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void DyingLight()
    {
        StartCoroutine("fadingLight", 0.3f);
    }

    IEnumerator fadingLight(float duration)
    {
        endFade = true;
        scalingFramesLeft = 130;

        yield return new WaitForSeconds(duration);

        GameObject.Find("GameManager").GetComponent<SlothGameManager>().NewLight();
    }
}
