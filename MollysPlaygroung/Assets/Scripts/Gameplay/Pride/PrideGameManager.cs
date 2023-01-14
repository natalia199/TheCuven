using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrideGameManager : MonoBehaviour
{
    public List<GameObject> cups = new List<GameObject>();
    public List<GameObject> PlayerTurns = new List<GameObject>();
    public List<GameObject> ChaliceSelection = new List<GameObject>();

    public int trackedTurn;

    public TextMeshProUGUI displayPlayerTurn;


    void Start()
    {
        trackedTurn = 0;

        StartRound();
    }

    void Update()
    {
        
    }

    void StartRound()
    {
        displayPlayerTurn.text = PlayerTurns[0].name + "'s Turn";

        PlayerTurns[0].GetComponent<PlayerPride>().ActivatePlayersMouse();
        PlayerTurns[1].GetComponent<PlayerPride>().DeactivatePlayersMouse();
    }

    public void Chalices(int cup)
    {
        ChaliceSelection.Add(cups[cup]);
        Debug.Log(ChaliceSelection.Count + "/2 Cup " + cups[cup] + " added by " + this.name);

        for (int i = 0; i< cups.Count; i++)
        {
            if(i == cup)
            {
                cups[i].GetComponent<ChalicePride>().cupState = true;
            }
            else
            {
                cups[i].GetComponent<ChalicePride>().cupState = false;
            }
        }

        if (trackedTurn == 1)
        {
            NextTurn();
        }
        else if(trackedTurn == 2)
        {
            CheckResults();
        }
    }

    public void NextTurn()
    {
        //Debug.Log("next turn");

        displayPlayerTurn.text = PlayerTurns[1].name + "'s Turn";

        PlayerTurns[0].GetComponent<PlayerPride>().DeactivatePlayersMouse();
        PlayerTurns[1].GetComponent<PlayerPride>().ActivatePlayersMouse();
    }

    public void CheckResults()
    {
        Debug.Log(ChaliceSelection[0].name + " vs " + ChaliceSelection[1].name);
        
        if(ChaliceSelection[0].name == ChaliceSelection[1].name)
        {
            DisplayResult(false);
        }
        else
        {
            DisplayResult(true);
        }
    }

    void SwitchRoles()
    {
        PlayerTurns.Reverse();
    }

    void ResetCups()
    {
        for(int i = 0; i < cups.Count; i++)
        {
            cups[i].GetComponent<ChalicePride>().cupState = false;
        }

        ChaliceSelection.RemoveAt(0);
        ChaliceSelection.RemoveAt(0);

        ChaliceSelection.Clear();
    }

    void DisplayResult(bool res)
    {
        PlayerTurns[0].GetComponent<PlayerPride>().DeactivatePlayersMouse();
        PlayerTurns[1].GetComponent<PlayerPride>().DeactivatePlayersMouse();

        if (res)
        {
            displayPlayerTurn.text = PlayerTurns[1].name + " is safe!";
            StartCoroutine("ResultAnouncement", 3f);
        }
        else
        {
            displayPlayerTurn.text = PlayerTurns[1].name + " died!";
        }
    }

    IEnumerator ResultAnouncement(float duration)
    {
        yield return new WaitForSeconds(duration);

        trackedTurn = 0;
        ResetCups();
        SwitchRoles();
        StartRound();
    }
}
