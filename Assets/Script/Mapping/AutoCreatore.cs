using System.Collections.Generic;
using Script.Mapping.ParcelType;
using Script.UI.Windows;
using Script.Vehicle;
using Script.Vehicle.VehicleData;
using UnityEngine;

namespace Script.Mapping
{
	public static class AutoCreatore
	{
		public static void MakeAuto(Vector2Int origine)
		{
			//Vector2Int origine = new Vector2Int(200, 200);
			for (int i = -30; i <= 0; i++)
			{
				MapManager.map.AddConstruction(origine + new Vector2Int(i, 1), new Road());
			}
			for (int i = 0; i <= 30; i++)
			{
				MapManager.map.AddConstruction(origine + new Vector2Int(0, i), new Road());
			}
			for (int i = -15; i <= 15; i+= 15)
			{

				Industrise indus = MapManager.map.CreatIndustrise(origine + new Vector2Int(-35, i));
				indus.industriseData = FIleSys.GetAllInstances<IndustriseData>()[0];
				indus.materialProductionRatio = 200;
				indus.SetInputeOutpure();
			}
			for (int i = -15; i <= 15; i+= 15)
			{
				Industrise indus = MapManager.map.CreatIndustrise(origine + new Vector2Int(i, 35));
				indus.industriseData = FIleSys.GetAllInstances<IndustriseData>()[1];
				indus.materialProductionRatio = 200;
				indus.SetInputeOutpure();
			}
		
			MapManager.map.AddConstruction(origine + new Vector2Int(-31, 1), new LoadingBay());
			MapManager.map.AddConstruction(origine + new Vector2Int(0, 31), new LoadingBay());

			MapManager.map.AddConstruction(origine, new Depot());
			Depot depot = MapManager.map.GetParcel<Depot>(origine);
			Debug.Log(depot);
			Debug.Log(FIleSys.GetAllInstances<VehicleData>()[1]);
			Group group = new Group()
			{
				name = "Auto Generate Groupe"
			};
			List<VehicleContoler> vehicles = new List<VehicleContoler>();
			for (int i = 0; i < 3; i++)
			{
				vehicles.Add(depot.BuyVehicle(FIleSys.GetAllInstances<VehicleData>()[1]));
				vehicles[i].MyGroup = Group.groups[0];
			}
			group.forceRoute = true;
			group.route = new Route()
			{
				points = new List<Vector2Int>()
				{
					origine + new Vector2Int(-31, 1),
					origine + new Vector2Int(0, 31),
					origine,
				}
			};
			Group.groups[0].StartEveryVehicle();
		}

	
	}
}
