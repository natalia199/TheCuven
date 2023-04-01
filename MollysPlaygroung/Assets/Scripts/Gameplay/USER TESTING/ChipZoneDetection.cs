using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChipZoneDetection : MonoBehaviour
{
    bool m_Started;

    public bool activatedForPlayer = true;

    public LayerMask m_LayerMask;

    public Collider[] zoneCollider;

    public TextMeshProUGUI scoreZoneTxt;

    public int chipsInZone;
    public int chipZoneID;

    void Start()
    {
        m_Started = true;
    }

    void Update()
    {
        if (activatedForPlayer)
        {
            chipsInZone = zoneCollider.Length / 2;

            scoreZoneTxt.text = zoneCollider.Length / 2 + "/" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips;
        }
        else
        {
            chipsInZone = 0;
        }
    }

    void FixedUpdate()
    {
        if (activatedForPlayer)
        {
            zoneCollider = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        }
    }
}
