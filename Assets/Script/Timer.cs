using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private float timer;

    public Timer()
    {
        timer = Time.time;
    }

    public float DebugTime(string action)
    {
        float returnTime = Time.time - timer;
        timer = Time.time;
        Debug.Log($"{returnTime}s need for {action}");
        return returnTime;
    }
}
