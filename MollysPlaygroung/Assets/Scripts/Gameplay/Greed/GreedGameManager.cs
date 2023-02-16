using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GreedGameManager : MonoBehaviour
{
    public Vector3 diceCameraPos;
    public Quaternion diceCameraRot;
    public float diceCameraSize;

    public Vector3 playCameraPos;
    public Quaternion playCameraRot;
    public float playCameraSize;

    public GameObject ChipPrefab;
    public GameObject ChipParent;
    public List<Transform> ChipSpawnPoints = new List<Transform>();
    public List<TextMeshProUGUI> ZoneChips = new List<TextMeshProUGUI>();

    public bool diceProcedure;

    public GameObject Dice;

    public int diceValueRolled;

    void Start()
    {
        diceProcedure = true;
        SetPlayersCamera();
        UpdateChipAmount(10);
    }

    void Update()
    {
        SetPlayersCamera();
    }

    public void SetPlayersCamera()
    {
        if (diceProcedure)
        {

            GameObject.Find("Main Camera").transform.position = diceCameraPos;
            GameObject.Find("Main Camera").transform.rotation = diceCameraRot;
            GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = diceCameraSize;
        }
        else
        {
            GameObject.Find("Main Camera").transform.position = playCameraPos;
            GameObject.Find("Main Camera").transform.rotation = playCameraRot;
            GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = playCameraSize;
        }
    }

    public void SetZoneChipCount(int num, int amount)
    {
        ZoneChips[num].text = "" + amount;
    }

    public void RollingTheDice()
    {
        Dice.GetComponent<Dice>().RollDice();
    }

    public void DiceValueUpdate(int value)
    {
        diceValueRolled = value;
    }

    public void FirstCarriedChip(GameObject chip, GameObject player, int chipCount)
    {
        Destroy(chip.GetComponent<Rigidbody>());

        chip.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);   
        chip.transform.rotation = Quaternion.identity;

        chip.transform.parent = player.transform;
        chip.GetComponent<ChipScript>().Available = false;
    }

    public void CarryMoreChips(GameObject chip, GameObject player, GameObject previousChip, int chipCount)
    {
        Destroy(chip.GetComponent<Rigidbody>());

        chip.transform.position = new Vector3(player.transform.position.x, previousChip.transform.position.y + 1f, player.transform.position.z);
        chip.transform.rotation = Quaternion.identity;

        chip.transform.parent = player.transform;
        chip.GetComponent<ChipScript>().Available = false;
    }

    public void DropTheChip(GameObject chip, GameObject player, GameObject zone)
    {
        chip.transform.parent = null;
        chip.AddComponent<Rigidbody>();
        chip.GetComponent<ChipScript>().Available = true;
        chip.GetComponent<ChipScript>().throwChip(zone.transform.GetChild(0).position, GameObject.Find("Player").transform.position, 300);
    }

    public void ChipInstantiation()
    {
        float xPos = Random.Range(ChipSpawnPoints[0].position.x, ChipSpawnPoints[2].position.x);
        float zPos = Random.Range(ChipSpawnPoints[0].position.z, ChipSpawnPoints[1].position.z);

        GameObject temp = Instantiate(ChipPrefab, new Vector3(xPos, ChipPrefab.transform.position.y, zPos), Quaternion.identity);
        temp.transform.parent = ChipParent.transform;
    }

    void UpdateChipAmount(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            ChipInstantiation();
        }
    }
}
