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
				MapManager.map.AddRoad(origine + new Vector2Int(i, 1));
			}
			for (int i = 0; i <= 30; i++)
			{
				MapManager.map.AddRoad(origine + new Vector2Int(0, i));
			}

			for (var i = 0; i < 2; i+= 1)
			{
				FactoryParcel factory =
					MapManager.map.CreatFactory(origine + (i == 0 ? new Vector2Int(i, 35) : new Vector2Int(-35, i)));
				if (factory == null)
					return;
				factory.data =
					Resources.Load<IndustriseData>(
						"ScriptableObject/Factories/" + (i == 0 ? "GodInput" : "GodOutput"));

				foreach (var production in factory.productions)
					production.production = 200f;
				
				factory.InitializationSecondary();
			}
		
			MapManager.map.AddConstruction(origine + new Vector2Int(-31, 1), new LoadingBay());
			MapManager.map.AddConstruction(origine + new Vector2Int(0, 31), new LoadingBay(), Parcel.Orientation.Down);

			MapManager.map.AddConstruction(origine, new Depot());
			Depot depot = MapManager.map.GetParcel<Depot>(origine);
			Debug.Log(depot);
			Debug.Log(FIleSys.GetAllInstances<VehicleData>()[1]);
			Group group = new Group()
			{
				name = "Auto Generate Group"
			};
			List<VehicleContoler> vehicles = new List<VehicleContoler>();
			for (int i = 0; i < 5; i++)
			{
				vehicles.Add(depot.BuyVehicle(Resources.Load<VehicleData>("ScriptableObject/Vehicles/GodTruck")));
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
