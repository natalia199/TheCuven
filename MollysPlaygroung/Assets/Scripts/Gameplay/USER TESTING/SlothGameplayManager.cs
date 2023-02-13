using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlothGameplayManager : MonoBehaviour
{
    public List<Transform> TrapSpawnPoints = new List<Transform>();

    public GameObject TrapPrefab;
    public GameObject TrapParent;
    public int AmountOfTraps;

    public GameObject LightPrefab;
    public GameObject LightParent;
    bool newLight;

    GameObject oldLight;
    GameObject currentLight;

    void Start()
    {
        newLight = true;

        LightInstantiation();
    }

    void Update()
    {
        UpdateTrapAmount();
    }

    // BEAR TRAPS
    public void TrapInstantiation()
    {
        StartCoroutine("HoldIt", Random.Range(0, 3));
    }
    void UpdateTrapAmount()
    {
        int x = AmountOfTraps - TrapParent.transform.childCount;
        for (int i = 0; i < x; i++)
        {
            TrapInstantiation();
        }
    }
    IEnumerator HoldIt(float time)
    {
        float xPos = Random.Range(TrapSpawnPoints[0].position.x, TrapSpawnPoints[2].position.x);
        float zPos = Random.Range(TrapSpawnPoints[0].position.z, TrapSpawnPoints[1].position.z);

        GameObject trap = Instantiate(TrapPrefab, new Vector3(xPos, 12f, zPos), Quaternion.identity, TrapParent.transform);

        yield return new WaitForSeconds(time);

        trap.AddComponent<Rigidbody>();
    }

    // LAMP LIGHT
    public void LightInstantiation()
    {
        StartCoroutine("LightLifeTime", Random.Range(5, 10));
    }

    IEnumerator LightLifeTime(float time)
    {
        float xPos = Random.Range(TrapSpawnPoints[0].position.x, TrapSpawnPoints[2].position.x);
        float zPos = Random.Range(TrapSpawnPoints[0].position.z, TrapSpawnPoints[1].position.z);

        GameObject light = Instantiate(LightPrefab, new Vector3(xPos, LightPrefab.transform.position.y, zPos), Quaternion.identity, LightParent.transform);

        currentLight = light;

        yield return new WaitForSeconds(time);

        currentLight.GetComponent<LightFlickerSloth>().LightsOut();
        currentLight = null;

        LightInstantiation();
    }
}
