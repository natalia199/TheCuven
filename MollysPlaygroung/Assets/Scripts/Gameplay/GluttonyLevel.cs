using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GluttonyLevel : MonoBehaviour
{
    public GameObject[] foodItems;
    public float spawnTime = 5f;
    public float spawnDelay = 5f;
    public bool stopSpawning = false;
    public float spawnHeight = 50f;
    public float[] mapSize = { 100f, 100f }; //This will determine item spawn range. If the size is 100 x 100, items will spawn between 50 and -50

    void Start()
    {
        InvokeRepeating("SpawnFood", spawnTime, spawnDelay);
    }

    void Update()
    {

    }

    public void SpawnFood()
    {

        if(foodItems.Length > 0)
        {
            int foodItemNum = Random.Range(0, foodItems.Length);
            Vector3 spawnPosition = new Vector3(Random.Range(-mapSize[0]/2, mapSize[0]/2), spawnHeight, Random.Range(-mapSize[1] / 2, mapSize[1] / 2));
            Instantiate(foodItems[foodItemNum], spawnPosition, transform.rotation);
        }
        
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
