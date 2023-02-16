using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChipZoneDetection : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;

    public Collider[] zoneCollider;

    void Start()
    {
        m_Started = true;
    }

    void Update()
    {
        GameObject.Find("ZoneCanvas").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + zoneCollider.Length/2;
    }

    void FixedUpdate()
    {
        zoneCollider = Physics.OverlapBox(gameObject.transform.position, transform.localScale/2, Quaternion.identity, m_LayerMask);
    }
}
