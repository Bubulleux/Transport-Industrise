using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VehicleManager 
{

    public static List<VehicleContoler> Vehicles
    {
        get
        {
            List<VehicleContoler> returnValue = new List<VehicleContoler>();
            foreach(GameObject curVehicle in GameObject.FindGameObjectsWithTag("Vehicle"))
            {
                returnValue.Add(curVehicle.GetComponent<VehicleContoler>());
            }
            return returnValue;
        }
    }

    public static List<VehicleContoler> GetVehicleByPos(Parcel parcel)
    {
        List<VehicleContoler> returnValue = new List<VehicleContoler>();
        foreach(VehicleContoler curVehicle in Vehicles)
        {
            if (curVehicle.VehiclePos == parcel.pos)
            {
                returnValue.Add(curVehicle);
            }
        }
        return returnValue;
    }
}
