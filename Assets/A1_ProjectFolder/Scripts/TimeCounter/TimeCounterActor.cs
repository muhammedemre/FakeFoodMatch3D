using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class TimeCounterActor : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeCounterText;
    [SerializeField] int leftDuration;
    bool count = false;
    float nextCountTime = 0;

    private void Update()
    {
        if (count)
        {
            if (nextCountTime < Time.time)
            {
                nextCountTime = Time.time + 1;
                leftDuration--;
                if (leftDuration == 0)
                {
                    TimeIsUP();
                }
                DisplayTheTime(leftDuration);
            }
        }
    }

    public void StartCountingTheTime() 
    {
        count = true;
    }
    public void StopCounting() 
    {
        count = false;
    }

    public void DisplayTheTime(int durationInSeconds) 
    {
        leftDuration = durationInSeconds;
        string timeString = ConvertSecondsToTimeString(durationInSeconds);
        timeCounterText.text = timeString;
    }

    public string ConvertSecondsToTimeString(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;
        return string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
    }

    public void TimeIsUP() 
    {
        count = false;
        LevelManager.instance.levelCreateOfficer.currentLevel.levelOperationOfficer.GoalReachCheck();
    }

    [Button("Start Counting")]
    void ButtonCountStart() 
    {
        StartCountingTheTime();
    }

    [Button("Stop Counting")]
    void ButtonCountStop() 
    {
        StopCounting();
    }
}
