using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LustGameplayManager : MonoBehaviour
{
    public List<GameObject> pianoKeys = new List<GameObject>();

    void Start()
    {
        StartCoroutine("NewKeyPick", 3);
    }

    void Update()
    {
        
    }

    IEnumerator NewKeyPick(float time)
    {
        int chosenKey = Random.Range(0, pianoKeys.Count);

        if (pianoKeys[chosenKey].GetComponent<LustPianoKey>().keyPressed)
        {
            yield return new WaitForSeconds(0);
        }
        else
        {
            // change key material
            pianoKeys[chosenKey].GetComponent<LustPianoKey>().selectedKey();
            yield return new WaitForSeconds(time);
        }

        pianoKeys[chosenKey].GetComponent<LustPianoKey>().deselectedKey();
        StartCoroutine("NewKeyPick", 3);
    }
}
