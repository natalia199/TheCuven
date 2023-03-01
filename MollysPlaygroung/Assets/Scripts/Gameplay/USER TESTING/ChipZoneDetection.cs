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

        scoreZoneTxt.text = "" + zoneCollider.Length/2;
    }

    void FixedUpdate()
    {
        zoneCollider = Physics.OverlapBox(gameObject.transform.position, transform.localScale/2, Quaternion.identity, m_LayerMask);
    }
}
