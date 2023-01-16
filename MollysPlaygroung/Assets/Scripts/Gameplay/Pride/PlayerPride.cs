using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPride : MonoBehaviour
{
    public bool myTurn;

    public int selectedCup;

    void Start()
    {

    }

    void Update()
    {
        //Check for mouse click 
        if (myTurn)
        {
            if (Input.GetKeyDown(KeyCode.Space) && selectedCup != 0)
            {
                GameObject.Find("GameManager").GetComponent<PrideGameManager>().trackedTurn++;
                GameObject.Find("GameManager").GetComponent<PrideGameManager>().Chalices(selectedCup - 1);
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit die;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out die, 100.0f))
                {
                    if (die.transform.tag == "Chalice")
                    {
                        CurrentClickedGameObject(die.transform.gameObject.GetComponent<ChalicePride>().cupID);
                    }
                }
            }
        }
    }
    public void CurrentClickedGameObject(int gameObject)
    {
        Debug.Log("Cup " + gameObject + " selected by " + this.name);
        selectedCup = gameObject;
        //gameObject.GetComponent<ChalicePride>().SelectedCup();
    }

    public int PickARandomCup()
    {
        return Random.Range(1, 3);
    }

    public void CurrentRandomGameObject()
    {
        selectedCup = PickARandomCup();

        Debug.Log("Cup " + selectedCup + " selected by " + this.name);

        GameObject.Find("GameManager").GetComponent<PrideGameManager>().Chalices(selectedCup - 1);
    }

    public void DeactivatePlayersMouse()
    {
        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        */
        myTurn = false;
        selectedCup = 0;
    }

    public void ActivatePlayersMouse()
    {
        /*Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        */
        myTurn = true;
        selectedCup = 0;
    }
}
