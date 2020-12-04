﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapLoader : MonoBehaviour
{
    private Task<Map> operation;
    public static bool load = false;
    public TaskStatus operationStatus;
    public void Awake()
    {
        if (load == true)
        {
            Destroy(gameObject);
            return;
        }
        load = true;
        DontDestroyOnLoad(gameObject);
        operation = AsyncLoadMap(GameObject.Find("Map").GetComponent<MapManager>().heightCurv, GameObject.Find("Map").GetComponent<MapManager>().limitWaterCurv);
        operationStatus = operation.Status;
        
    }

    public async Task<Map> AsyncLoadMap(AnimationCurve heightCurv, AnimationCurve limitWaterCurv)
    {
        float startTime = System.DateTime.Now.Second + System.DateTime.Now.Minute * 60;
        SceneManager.LoadSceneAsync(1);
        Map _mapdata = new Map();
        await _mapdata.GenerateMap(heightCurv, limitWaterCurv);
        //await Task.Delay(3000);
        SceneManager.LoadSceneAsync(0);
        MapManager.map = _mapdata;
        //operation = null;
        Debug.Log("Load End");
        Destroy(gameObject);
        Debug.Log($"time to creat Map: {System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 - startTime}");
        return _mapdata;
    }

    private void Update()
    {
        if (operationStatus != TaskStatus.Faulted && operation.Status == TaskStatus.Faulted)
        {
            operationStatus = operation.Status;
            Debug.LogError($"message: {operation.Exception.Message}, \n source: {operation.Exception.Source}, \n\n comple: {operation.Exception.ToString()}");
        }
    }
}
