using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AutoCratore
{
    public static void MakeAuto(Vector2Int origine)
    {
        //Vector2Int origine = new Vector2Int(200, 200);
        MapManager.map.AddConstruction(origine, new Depot());
        for (int i = -30; i < 31; i++)
        {
            MapManager.map.AddConstruction(origine + new Vector2Int(i, 1), new Road());
        }
        Industrise indus1 = MapManager.map.CreatIndustrise(origine + new Vector2Int(-50, 0));
        Industrise indus2 = MapManager.map.CreatIndustrise(origine + new Vector2Int(50, 0));
        indus1.industriseData = FIleSys.GetAllInstances<IndustriseData>()[0];
        indus2.industriseData = FIleSys.GetAllInstances<IndustriseData>()[1];
        indus1.SetInputeOutpure();
        indus2.SetInputeOutpure();
        MapManager.map.AddConstruction(origine + new Vector2Int(-31, 1), new LoadingBay());
        MapManager.map.AddConstruction(origine + new Vector2Int(31, 1), new LoadingBay());
        Depot depot = MapManager.map.parcels[origine.x, origine.y] as Depot;
        VehicleContoler vehicle1 = depot.BuyVehicle(FIleSys.GetAllInstances<VehicleData>()[0]);
        VehicleContoler vehicle2 = depot.BuyVehicle(FIleSys.GetAllInstances<VehicleData>()[0]);
        new Groupe()
        {
            name = "Auto Generate Groupe",
            vehicles = new List<VehicleContoler>()
            {
                vehicle1,
                vehicle2
            }
        };
        vehicle1.groupe = Groupe.groupes[0];
        vehicle2.groupe = Groupe.groupes[0];
        vehicle1.route = new Route()
        {
            points = new List<Vector2Int>()
            {
                origine + new Vector2Int(-31, 1),
                origine + new Vector2Int(31, 1)
            }
        };
        Groupe.groupes[0].StartEveryVehicle();
    }

    
}
