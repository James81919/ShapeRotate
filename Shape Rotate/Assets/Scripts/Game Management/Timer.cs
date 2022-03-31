using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public string timerName;
    public System.TimeSpan duration;

    public Timer(string _timerName, System.TimeSpan _duration)
    {
        timerName = _timerName;
        duration = _duration;
    }

    public System.DateTime GetFinishTime()
    {
        return System.DateTime.Parse(PlayerPrefs.GetString(timerName, System.DateTime.Now.ToString()));
    }

    public System.TimeSpan GetTimeRemaining()
    {
        return GetFinishTime() - System.DateTime.Now;
    }

    public bool HasTimerEnded()
    {
        return GetTimeRemaining() <= System.TimeSpan.Zero;
    }

    public void StartTimer()
    {
        PlayerPrefs.SetString(timerName, System.DateTime.Now.Add(duration).ToString());
    }
}
