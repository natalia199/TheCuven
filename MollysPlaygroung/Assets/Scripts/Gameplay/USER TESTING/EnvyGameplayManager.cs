using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvyGameplayManager : MonoBehaviour
{
    public List<GameObject> racingHorseys = new List<GameObject>();
    public List<GameObject> horseResults = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
        {
            if (racingHorseys.Count == horseResults.Count)
            { 
                GetComponent<TempLevelTimer>().CallGameEnd();                
            }
        }*/
    }

    public void RecordHorseResult(GameObject horse)
    {
        horseResults.Add(horse);
    }

    public void endingGameDelay()
    {
        StartCoroutine("HoldIt", 5);
    }

    IEnumerator HoldIt(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState = true;
    }

    public void MoveHorseForward(string name)
    {
        GameObject.Find(name).GetComponent<EnvyHorse>().MoveYourHorse();
    }

    public void StopHorseForward(string name)
    {
        GameObject.Find(name).GetComponent<EnvyHorse>().StopYourHorse();
    }
    public void squirtWater(string name)
    {
        GameObject.Find(name).transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = true;
    }

    public void desquirtWater(string name)
    {
        GameObject.Find(name).transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = false;
    }

    public void MoveHorseForwardSP(GameObject gun)
    {
        GameObject.Find(gun.transform.GetChild(0).GetComponent<EnvySquirter>().correlatingHorse).GetComponent<EnvyHorse>().MoveYourHorse();
    }

    public void StopHorseForwardSP(GameObject gun)
    {
        GameObject.Find(gun.transform.GetChild(0).GetComponent<EnvySquirter>().correlatingHorse).GetComponent<EnvyHorse>().StopYourHorse();
    }
    public void squirtWateSP(GameObject gun)
    {
        gun.transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = true;
    }

    public void desquirtWaterSP(GameObject gun)
    {
        gun.transform.GetChild(0).GetComponent<EnvySquirter>().squirterActivated = false;
    }
}
