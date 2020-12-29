using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private DateTime timeRef;
    private List<double> times =  new List<double>();

    public Timer()
    {
        ResetTimer();
    }
    public void ResetTimer()
    {
        timeRef = DateTime.Now;
    }
    public double GetTimer()
    {
        return (DateTime.Now - timeRef).TotalSeconds;
    }

    public double DebugTime(string action)
    {
        double returnTime = GetTimer();
        ResetTimer();
        Debug.Log($"{returnTime}s need for {action}");
        return returnTime;
    }
    public void AddTime()
    {
        times.Add(GetTimer());
        ResetTimer();
    }
    public double GetTimesMoyen()
    {
        return times.Sum() / times.Count;
    }
    public void ClearTime()
    {
        times.Clear();
    }
}
