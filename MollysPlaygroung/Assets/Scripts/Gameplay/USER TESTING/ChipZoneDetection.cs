using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChipZoneDetection : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;

    public Collider[] zoneCollider;

    public TextMeshProUGUI scoreZoneTxt;

    public int chipsInZone;

    void Start()
    {
        m_Started = true;
    }

    void Update()
    {
        chipsInZone = zoneCollider.Length / 2;

        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            scoreZoneTxt.text = zoneCollider.Length / 2 + "/" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().AmountOfChips;
        }
        else
        {
            scoreZoneTxt.text = zoneCollider.Length / 2 + "/" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips;
        }
    }

    void FixedUpdate()
    {
        zoneCollider = Physics.OverlapBox(gameObject.transform.position, transform.localScale/2, Quaternion.identity, m_LayerMask);
    }
}
