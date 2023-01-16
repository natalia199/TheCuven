using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalicePride : MonoBehaviour
{
    public bool cupState;
    public string cupName;

    public int cupID;

    public List<GameObject> otherCups = new List<GameObject>();

    void Start()
    {
        this.name = cupName;
        cupState = false;
    }

    void Update()
    {
        
    }

    public void SelectedCup()
    {
        cupState = true;
        
        for(int i = 0; i < otherCups.Count; i++)
        {
            otherCups[i].GetComponent<ChalicePride>().cupState = false;
        }
    }
}
