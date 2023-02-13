using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneGreed : MonoBehaviour
{
    public int chipAmount;
    public int ID;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "ChipInZone" && col.tag != "Chip")
        {
            Debug.Log("in");
            chipAmount++;
            GameObject.Find("GameManager").GetComponent<GreedGameManager>().SetZoneChipCount(ID, chipAmount);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "ChipInZone" && col.tag != "Chip")
        {
            Debug.Log("out");
            chipAmount--;
            GameObject.Find("GameManager").GetComponent<GreedGameManager>().SetZoneChipCount(ID, chipAmount);
        }
    }
}
