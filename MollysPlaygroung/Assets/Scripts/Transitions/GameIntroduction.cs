using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameIntroduction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;                                // Syncing all players views once they're in a room

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("GameIntroTime", 5f);
        }
    }

    IEnumerator GameIntroTime(int value)
    {
        Debug.Log("game intro playing");

        yield return new WaitForSeconds(value);

        Debug.Log("game intro DONE");
        //PhotonNetwork.LoadLevel("Transition");
        
        //PhotonNetwork.LoadLevel("Wrath");
        PhotonNetwork.LoadLevel("Envy");
    }
}
