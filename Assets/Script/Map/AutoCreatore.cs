using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AutoCreatore
{
    public static void MakeAuto(Vector2Int origine)
    {
        //Vector2Int origine = new Vector2Int(200, 200);
        MapManager.map.AddConstruction(origine + new Vector2Int(0, 1), new Depot());
        for (int i = -30; i <= 0; i++)
        {
            MapManager.map.AddConstruction(origine + new Vector2Int(i, 1), new Road());
        }
        for (int i = 0; i <= 30; i++)
        {
            MapManager.map.AddConstruction(origine + new Vector2Int(0, i), new Road());
        }
        Industrise indus1 = MapManager.map.CreatIndustrise(origine + new Vector2Int(-50, 0));
        Industrise indus2 = MapManager.map.CreatIndustrise(origine + new Vector2Int(0, 50));
        indus1.industriseData = FIleSys.GetAllInstances<IndustriseData>()[0];
        indus2.industriseData = FIleSys.GetAllInstances<IndustriseData>()[1];
        indus1.materialProductionRatio = 200;
        indus2.materialProductionRatio = 200;
        indus1.SetInputeOutpure();
        indus2.SetInputeOutpure();
        MapManager.map.AddConstruction(origine + new Vector2Int(-31, 1), new LoadingBay());
        MapManager.map.AddConstruction(origine + new Vector2Int(0, 31), new LoadingBay());
        Depot depot = MapManager.map.parcels[origine.x, origine.y + 1] as Depot;
        Group group = new Group()
        {
            name = "Auto Generate Groupe"
        };
        List<VehicleContoler> vehicles = new List<VehicleContoler>();
        for (int i = 0; i < 3; i++)
        {
            vehicles.Add(depot.BuyVehicle(FIleSys.GetAllInstances<VehicleData>()[0]));
            vehicles[i].MyGroup = Group.groups[0];
        }
        group.forceRoute = true;
        group.route = new Route()
        {
            points = new List<Vector2Int>()
                {
                    origine + new Vector2Int(-31, 1),
                    origine + new Vector2Int(0, 31)
                }
        };
        Group.groups[0].StartEveryVehicle();
    }

    
}
