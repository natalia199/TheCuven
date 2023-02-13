using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlothGameManager : MonoBehaviour
{
    public GameObject lifeBar;

    // LIGHT
    public GameObject LightPrefab;
    public List<Transform> LightSpawnPoints = new List<Transform>();
    GameObject currentLight;

    // OBSTACLE
    public GameObject ObstaclePrefab;
    public GameObject ObstacleParent;
    public List<Transform> ObstacleSpawnPoints = new List<Transform>();

    void Start()
    {
        // get amount of players and create a life bar for each one and name it -username- + "LifeBar"
        // also set their username and image in life bar

        UpdateObstacleAmount();
        NewLight();
    }

    void Update()
    {
        UpdateObstacleAmount();
    }

    public void DisplayerLifeBar(int score)
    {
        lifeBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = score + "";

        if (score >= 100) {
            lifeBar.transform.GetChild(1).gameObject.transform.position = new Vector3(400f, lifeBar.transform.GetChild(1).gameObject.transform.position.y, lifeBar.transform.GetChild(1).gameObject.transform.position.z) ;
        }
        else if(score > 9 && score < 100)
        {
            lifeBar.transform.GetChild(1).gameObject.transform.position = new Vector3(340f, lifeBar.transform.GetChild(1).gameObject.transform.position.y, lifeBar.transform.GetChild(1).gameObject.transform.position.z);
        }
        else
        {
            lifeBar.transform.GetChild(1).gameObject.transform.position = new Vector3(270f, lifeBar.transform.GetChild(1).gameObject.transform.position.y, lifeBar.transform.GetChild(1).gameObject.transform.position.z);
        }
    }

    public void NewLight()
    {
        StartCoroutine("LightInstantiationFlow", Random.Range(5, 10));
    }

    public void LightInstantiation()
    {
        float xPos = Random.Range(LightSpawnPoints[0].position.x, LightSpawnPoints[2].position.x);
        float zPos = Random.Range(LightSpawnPoints[0].position.z, LightSpawnPoints[1].position.z);

        currentLight = Instantiate(LightPrefab, new Vector3(xPos, LightPrefab.transform.position.y, zPos), Quaternion.identity);
    }

    public void ObstacleInstantiation()
    {
        float xPos = Random.Range(ObstacleSpawnPoints[0].position.x, ObstacleSpawnPoints[2].position.x);
        float zPos = Random.Range(ObstacleSpawnPoints[0].position.z, ObstacleSpawnPoints[1].position.z);

        Instantiate(ObstaclePrefab, new Vector3(xPos, 12f, zPos), Quaternion.identity, ObstacleParent.transform);
    }

    public void ActivateTrap(GameObject obstacle, GameObject player)
    {
        Destroy(obstacle.GetComponent<BoxCollider>());
        Destroy(obstacle.GetComponent<Rigidbody>());
        obstacle.transform.position = new Vector3(player.transform.position.x, obstacle.transform.position.y, player.transform.position.z);
        obstacle.GetComponent<SlothObstacle>().InitiateTrap();
    }

    IEnumerator LightInstantiationFlow(int value)
    {
        LightInstantiation();

        yield return new WaitForSeconds(value);

        currentLight.GetComponent<SlothLight>().DyingLight();
        currentLight = null;
    }

    void UpdateObstacleAmount()
    {
        if(ObstacleParent.transform.childCount < 3)
        {
            for(int i = 0; i < (3 - ObstacleParent.transform.childCount); i++)
            {
                ObstacleInstantiation();
            }
        }
    }
}
