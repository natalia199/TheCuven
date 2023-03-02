using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GluttonyGameplayManager : MonoBehaviour
{
    public List<Transform> FoodSpawnPoints = new List<Transform>();
    public List<GameObject> LifeSlots = new List<GameObject>();

    public GameObject FoodPrefab;
    public GameObject FoodParent;

    public int AmountOfFood;
    //public int AmountOfFoodSP;

    public Vector2 foodPosition;
    public bool foodReady = true;
    public Vector2 vomitedFoodPosition;
    public bool vomitedFoodReady = true;

    public float force;
    public bool noMoreFoodNeeded = false;
    public int foodInstantiationTracker;

    public int singlePlayerFinishedState;

    void Start()
    {
        foodReady = true;
        noMoreFoodNeeded = false;
        foodInstantiationTracker = FoodParent.transform.childCount;
        singlePlayerFinishedState = 0;
    }


    void Update()
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            try
            {
                LifeSlots[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;
                LifeSlots[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munched: " + GameObject.Find(PhotonNetwork.LocalPlayer.NickName).GetComponent<PlayerUserTest>().collectedFoodies + "/" + AmountOfFood;
            }
            catch (NullReferenceException e)
            {
                // error
            }

            /*
            // Instantiation
            if (AmountOfFoodSP >= FoodParent.transform.childCount && !noMoreFoodNeeded)
            {
                addFood(null);
            }

            if (FoodParent.transform.childCount == AmountOfFoodSP)
            {
                noMoreFoodNeeded = true;
            }
            */
        }
        else
        {
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                try
                {
                    LifeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i];
                    LifeSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Muched: " + GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i]).GetComponent<PlayerUserTest>().collectedFoodies + "/" + AmountOfFood;
                }
                catch (NullReferenceException e)
                {
                    // error
                }
            }

            /*
            try
            {
                // because of AmountOfTraps != TrapParent.transform.childCount, the last trap doesn't get a rigidbody and is on standby, so if u want X amount of traps on the field input a value of X+1
                if (foodReady && GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theFood != null && AmountOfFood != FoodParent.transform.childCount)
                {
                    addFood(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theFood);
                    foodReady = false;
                }
            }
            catch (NullReferenceException e)
            {
                // error
            }

            try
            {
                if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theVomittedFood != null)
                {
                    //addVomitedFood(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theVomittedFood);
                    //vomitedFoodReady = false;
                }
            }
            catch (NullReferenceException e)
            {
                // error
            }
            */
        }
    }

    public void addFood(GameObject food)
    {
        Debug.Log("food instant");

        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            foodInstantiationTracker++;
            food.AddComponent<Rigidbody>();
            FoodInstantiation();
        }
        else
        {
            StartCoroutine("FoodLifeTimeSP", 0);
        }
    }

    // BEAR TRAPS
    public void FoodInstantiation()
    {
        StartCoroutine("FoodLifeTime", 0);
    }

    IEnumerator FoodLifeTime(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theFood = null;
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().instantiateFoodOnce = false;
        foodReady = true;
    }

    IEnumerator FoodLifeTimeSP(float time)
    {
        float xPos = UnityEngine.Random.Range(FoodSpawnPoints[0].position.x, FoodSpawnPoints[2].position.x);
        float zPos = UnityEngine.Random.Range(FoodSpawnPoints[0].position.z, FoodSpawnPoints[1].position.z);
        Vector3 posy = new Vector3(xPos, 12, zPos);

        GameObject food = Instantiate(FoodPrefab, posy, Quaternion.identity, FoodParent.transform);

        yield return new WaitForSeconds(time);

        food.AddComponent<Rigidbody>();
    }

    public void incSinglePlayerState()
    {
        singlePlayerFinishedState++;
    }


    /*
    public void addFood(GameObject food)
    {
        food.AddComponent<Rigidbody>();
        FoodInstantiation();
    }

    public void addVomitedFood(GameObject food)
    {
        //food.AddComponent<Rigidbody>();
        VomitInstantiation();
    }

    public void FoodInstantiation()
    {
        StartCoroutine("HoldIt", 2);
    }

    IEnumerator HoldIt(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theFood = null;
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().instantiateFoodOnce = false;
        foodReady = true;
    }

    public void VomitInstantiation()
    {
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().theVomittedFood = null;
    }
    */

    /*
    public void VomittedFood(Vector3 playerPos)
    {
        GameObject food = Instantiate(FoodPrefab, new Vector3(playerPos.x, playerPos.y + 1.5f, playerPos.z), Quaternion.identity, FoodParent.transform);
        food.AddComponent<Rigidbody>();

        throwParabola(food);
    }

    void throwParabola( GameObject food)
    {
        float xVal = Random.Range(-5, 5);
        while(xVal < -1 || xVal > 1)
        {
            xVal = Random.Range(-5, 5);
        }

        float yVal = Random.Range(-5, 5);
        while (yVal < -1 || yVal > 1)
        {
            yVal = Random.Range(-5, 5);
        }

        Vector3 direction = new Vector3(xVal, Random.Range(3,5), yVal);
        direction = direction.normalized;

        // dir = dir.normalized;
        food.GetComponent<Rigidbody>().AddForce(direction * force);
        food.GetComponent<Rigidbody>().AddTorque(direction * 50);
    }

    IEnumerator HoldIt(float time)
    {
        float xPos = Random.Range(FoodSpawnPoints[0].position.x, FoodSpawnPoints[2].position.x);
        float zPos = Random.Range(FoodSpawnPoints[0].position.z, FoodSpawnPoints[1].position.z);

        GameObject food = Instantiate(FoodPrefab, new Vector3(xPos, 12f, zPos), Quaternion.identity, FoodParent.transform);

        yield return new WaitForSeconds(time);

        food.AddComponent<Rigidbody>();
    }
*/
}
