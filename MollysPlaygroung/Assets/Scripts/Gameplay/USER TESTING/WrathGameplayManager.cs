using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WrathGameplayManager : MonoBehaviour
{
    public int directionIndex;
    public bool returnHome;

    public GameObject lava;
    public float rotationsPerMinute;

    public GameObject wrathPlate;

    public bool plateState = false;

    public int AmountOfBoxes;
    public int TrackBoxes;
    public GameObject boxParent;
    public List<Transform> BoxpawnPoints = new List<Transform>();
    public GameObject boxPrefab;

    public GameObject singlePlayerPlatform;
    public GameObject multiPlayerPlatform;
    public GameObject boxScoreTxt;
    public bool noMoreBoxesNeeded = false;

    void Start()
    {
        plateState = wrathPlate.GetComponent<PlatformShakeWrath>().newTilt;

        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            singlePlayerPlatform.SetActive(true);
            multiPlayerPlatform.SetActive(false);
        }
        else
        {
            boxScoreTxt.SetActive(false);
            singlePlayerPlatform.SetActive(false);
            multiPlayerPlatform.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer && AmountOfBoxes > TrackBoxes && !noMoreBoxesNeeded)
        {
            // Instantiation
            for (int i = 0; i < (AmountOfBoxes-TrackBoxes); i++)
            {
                float xPos = UnityEngine.Random.Range(BoxpawnPoints[0].position.x, BoxpawnPoints[2].position.x);
                float zPos = UnityEngine.Random.Range(BoxpawnPoints[0].position.z, BoxpawnPoints[1].position.z);
                Vector3 posy = new Vector3(xPos, 5, zPos);

                Instantiate(boxPrefab, posy, Quaternion.identity, boxParent.transform);
                TrackBoxes++;
            }
        }

        lava.transform.Rotate(0, 6 * rotationsPerMinute * Time.deltaTime, 0);

        plateState = wrathPlate.GetComponent<PlatformShakeWrath>().newTilt;
    }

    public void shakePlateVariables(int dir)
    {
        wrathPlate.GetComponent<PlatformShakeWrath>().newTilt = false;
        plateState = false;
        wrathPlate.GetComponent<PlatformShakeWrath>().setVars(dir);
        GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().MasterPlayer).GetComponent<PlayerUserTest>().plateMoving = false;
    }
}
