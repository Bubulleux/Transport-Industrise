using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class AsyncTask : MonoBehaviour
{
    public static DateTime lastRender;
    private static double maxTimeWait = 0.5d;


    private void FixedUpdate()
    {
        lastRender = DateTime.Now;
    }
    public static async Task DelayIfNeed(int timeToWait)
    {
        if ((DateTime.Now - lastRender).TotalSeconds >= maxTimeWait)
        {
            await Task.Delay(timeToWait);
        }
    }

    public static async Task MonitorTask(Task task)
    {
        while(!task.IsCompleted && !task.IsFaulted)
        {
            await Task.Delay(1);
        }
        if(task.IsFaulted)
        {
            Debug.LogException(task.Exception);
        }
    }

}
