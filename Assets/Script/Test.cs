using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Awake()
    {
        foreach (VehicleData curVehicleData in FIleSys.GetAllInstances<VehicleData>())
        {
            Debug.Log(curVehicleData.name);
        }
    }
}


