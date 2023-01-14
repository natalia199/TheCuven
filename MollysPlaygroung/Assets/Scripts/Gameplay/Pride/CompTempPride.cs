using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompTempPride : MonoBehaviour
{
    public bool myTurn;

    public int selectedCup;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int PickARandomCup()
    {
        return Random.Range(0, 2);
    }

    public void CurrentRandomGameObject()
    {
        selectedCup = PickARandomCup();

        Debug.Log("Cup " + selectedCup + " selected by " + this.name);

        selectedCup = selectedCup - 1;

        GameObject.Find("GameManager").GetComponent<PrideGameManager>().Chalices(selectedCup);
    }

    public void DeactivatePlayersMouse()
    {
        myTurn = false;
        selectedCup = 0;
    }

    public void ActivatePlayersMouse()
    {
        CurrentRandomGameObject();

        myTurn = true;
        selectedCup = 0;
    }
}
