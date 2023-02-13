using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyGameplayManager : MonoBehaviour
{
    public List<Transform> FoodSpawnPoints = new List<Transform>();

    public GameObject FoodPrefab;
    public GameObject FoodParent;

    public int AmountOfFood;

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


    IEnumerator HoldIt(float time)
    {
        float xPos = Random.Range(FoodSpawnPoints[0].position.x, FoodSpawnPoints[2].position.x);
        float zPos = Random.Range(FoodSpawnPoints[0].position.z, FoodSpawnPoints[1].position.z);

        GameObject food = Instantiate(FoodPrefab, new Vector3(xPos, 12f, zPos), Quaternion.identity, FoodParent.transform);

        yield return new WaitForSeconds(time);

        food.AddComponent<Rigidbody>();
    }

}
