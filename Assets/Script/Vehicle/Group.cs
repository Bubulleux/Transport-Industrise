using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Group
{
    public static List<Group> groups = new List<Group>();
    public string name;
    public List<VehicleContoler> vehicles = new List<VehicleContoler>();

    public bool forceRoute;
    public Route route;

    public Group()
    {
        groups.Add(this);
        name = "Random Groupe";
    }


    public async Task StartEveryVehicle()
    {
        foreach(VehicleContoler curVehicle in vehicles)
        {
            curVehicle.StartVehicle();
            await Task.Delay(500);
        }
    }

    public void StopEveryVehicle()
    {
        foreach (VehicleContoler curVehicle in vehicles)
        {
            curVehicle.ReturnInDepot();
        }
    }
}
