using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GluttonyLevel : MonoBehaviour
{
    public GameObject foodItem;
    public float spawnTime = 5f;
    public float spawnDelay = 5f;
    public bool stopSpawning = false;

    void Start()
    {
        InvokeRepeating("SpawnFood", spawnTime, spawnDelay);
    }

    void Update()
    {

    }

    public void SpawnFood()
    {
        Instantiate(foodItem, transform.position, transform.rotation);
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
