using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer: MonoBehaviour {
    public float TimeLeft;
    public bool TimerOn = false;

    public TMP_Text TimerTxt;

    void Start () {
        TimerOn = true;
    }

    void Update () {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            } else
            {
                Debug.Log("Time is UP!");
                TimeLeft = 0;
                TimerOn = false;
            }
        }
    }

    void updateTimer (float currentTime) {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}