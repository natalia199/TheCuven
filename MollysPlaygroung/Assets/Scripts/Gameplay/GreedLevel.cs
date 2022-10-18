using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GreedLevel : MonoBehaviour
{
    public float timeLeft = 10.0f;
    public TextMeshProUGUI countdownText;

    void Start()
    {
        countdownText.text = "10s";
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            countdownText.text = Mathf.RoundToInt(timeLeft) + "s";
        }
    }
}
