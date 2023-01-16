using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvyGameManager : MonoBehaviour
{
    public List<GameObject> playingPlayers = new List<GameObject>();
    public List<GameObject> raceHorses = new List<GameObject>();

    public List<string> raceResults = new List<string>();

    void Start()
    {
        SetPlayerHorses();
    }

    void Update()
    {
        
    }

    public void SetPlayerHorses()
    {
        List<string> temp = new List<string>();

        for (int i = 0; i < raceHorses.Count; i++)
        {
            temp.Add(raceHorses[i].name);
        }

        for (int i = 0; i < playingPlayers.Count; i++)
        {
            for (int y = 0; y < temp.Count; y++)
            {
                Debug.Log(temp[y]);
            }

            int index = Random.Range(0, (temp.Count-1));
            playingPlayers[i].GetComponent<PlayerEnvy>().horseName = temp[index];
            //Debug.Log(temp[index] + " is taken and removed");
            temp.RemoveAt(index);
        }
    }

    public void MoveHorse(string horse)
    {
        for(int i = 0; i < raceHorses.Count; i++)
        {
            if(raceHorses[i].name == horse)
            {
                raceHorses[i].GetComponent<HorseMovement>().race = true;
            }
        }
    }

    public void StopHorse(string horse)
    {
        for (int i = 0; i < raceHorses.Count; i++)
        {
            if (raceHorses[i].name == horse)
            {
                raceHorses[i].GetComponent<HorseMovement>().race = false;
            }
        }
    }

    public void UpdateRaceResult(string name)
    {
        for (int i = 0; i < raceHorses.Count; i++)
        {
            if(raceHorses[i].name == name)
            {
                raceHorses[i].GetComponent<HorseMovement>().finished = true;
                raceHorses[i].GetComponent<HorseMovement>().race = true;
            }
        }
        raceResults.Add(name);
    }
}
