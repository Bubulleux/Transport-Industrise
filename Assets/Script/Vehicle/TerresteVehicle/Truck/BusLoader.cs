using System.Collections.Generic;
using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine;

namespace Script.Vehicle.TerresteVehicle.Truck
{
	public class BusLoader : VehicleLoader
	{
		private Dictionary<Vector2Int, int> peopleDestination = new Dictionary<Vector2Int, int>();

		public override float Load()
		{

			if (!MapManager.map.ParcelIs<BusStop>(vehicleControler.VehiclePos))
				return 0f;
			float time = 0f;
			
			BusStop busStop = MapManager.map.GetParcel<BusStop>(vehicleControler.VehiclePos);
			if (peopleDestination.ContainsKey(vehicleControler.VehiclePos))
			{
				busStop.Unload(peopleDestination[vehicleControler.VehiclePos]);
				peopleDestination.Remove(vehicleControler.VehiclePos);
				time += 0.05f;
			}

			foreach (var destination in busStop.LoadPeople(vehicleControler.vehicleData.maxProductTransport - PeopleInTheBus(), vehicleControler.MyRoute))
			{
				if (peopleDestination.ContainsKey(destination))
					peopleDestination[destination] += 1;
				else 
					peopleDestination.Add(destination, 1);
				time += 0.05f;
			}

			return time;
		}

		public int PeopleInTheBus()
		{
			int sum = 0;
			foreach (var destination in peopleDestination)
			{
				sum += destination.Value;
			}

			return sum;
		}
	}
}