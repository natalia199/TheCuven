using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvyGameplayManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
