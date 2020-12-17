using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groupe
{
    public static List<Groupe> groupes = new List<Groupe>();
    public string name;
    public List<VehicleContoler> vehicles = new List<VehicleContoler>();

    public Groupe()
    {
        groupes.Add(this);
        name = "Random Groupe";
    }

    public void StartEveryVehicle()
    {
        Debug.Log("Vehicle count:" + vehicles.Count);
        foreach(VehicleContoler curVehicle in vehicles)
        {
            curVehicle.StartVehicle();
            Debug.Log($"Vehicle: {curVehicle.id}, state: {curVehicle.state}");
        }
    }
    public void StopEveryVehicle()
    {
        foreach (VehicleContoler curVehicle in vehicles)
        {
            curVehicle.ReturnInDepot();
            Debug.Log($"Vehicle: {curVehicle.id}, state: {curVehicle.state}");
        }
    }
}
