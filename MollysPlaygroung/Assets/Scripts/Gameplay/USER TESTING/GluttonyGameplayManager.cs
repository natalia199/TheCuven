using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyGameplayManager : MonoBehaviour
{
    public List<Transform> FoodSpawnPoints = new List<Transform>();

    public GameObject FoodPrefab;
    public GameObject FoodParent;

    public int AmountOfFood;

    public float force;
    void Start()
    {
        
    }


    void Update()
    {
        UpdateFoodAmount();
    }

    public void FoodInstantiation()
    {
        StartCoroutine("HoldIt", Random.Range(0, 3));
    }

    void UpdateFoodAmount()
    {
        int x = AmountOfFood - FoodParent.transform.childCount;
        for (int i = 0; i < x; i++)
        {
            FoodInstantiation();
        }
    }

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

}
