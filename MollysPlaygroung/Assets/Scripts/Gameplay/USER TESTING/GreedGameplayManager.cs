using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedGameplayManager : MonoBehaviour
{
    public List<Transform> ChipSpawnPoints = new List<Transform>();

    public GameObject ChipPrefab;
    public GameObject ChipParent;

    public int AmountOfChips;

    void Start()
    {

    }


    void Update()
    {
        UpdateChipAmount();
    }

    public void ChipInstantiation()
    {
        StartCoroutine("HoldIt", Random.Range(0, 3));
    }

    void UpdateChipAmount()
    {
        int x = AmountOfChips - ChipParent.transform.childCount;
        for (int i = 0; i < x; i++)
        {
            ChipInstantiation();
        }
    }


    IEnumerator HoldIt(float time)
    {
        float xPos = Random.Range(ChipSpawnPoints[0].position.x, ChipSpawnPoints[2].position.x);
        float zPos = Random.Range(ChipSpawnPoints[0].position.z, ChipSpawnPoints[1].position.z);

        GameObject chip = Instantiate(ChipPrefab, new Vector3(xPos, 12f, zPos), Quaternion.identity, ChipParent.transform);

        chip.transform.rotation = new Quaternion(Random.Range(15, -15), 0, Random.Range(15, -15), chip.transform.rotation.w);

        yield return new WaitForSeconds(time);

        chip.AddComponent<Rigidbody>();
    }
}
