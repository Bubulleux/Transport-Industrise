using System;
using System.Collections.Generic;
using Script.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Mapping.ParcelType
{
	public class BusStop : Road, ITradePeople
	{
		public int peopleWait;
		public const int BusStopActionRadius = 4;
		public const int MAXPeopleWaiting = 100;


		public List<Dwelling> Dwellings { get
			{
				var dwellings = new List<Dwelling>();

				foreach (var posArea in Helper.GetCircleArea(pos, BusStopActionRadius))
				{
					if (map.ParcelIs<Dwelling>(posArea))
						dwellings.Add(map.GetParcel<Dwelling>(posArea));
				}
				return dwellings;
			} 
		}

		public override void InitializationSecondary()
		{
			color = Color.white;
			EnableUpdate(true);
			prefab = Resources.Load<GameObject>("ParcelGFX/BusStop");
		}
		
		public override void DebugParcel()
		{
			Debug.Log(peopleWait);
		}
		
		public override bool CanConnect(Vector2Int connectionPos)
		{
			return base.CanConnect(connectionPos) ||
			       Array.IndexOf(MapManager.parcelAround, pos - connectionPos) == (int)orientation;
		}

		public int GetPeople()
		{
			return peopleWait;
		}

		public void Unload(int peopleCount)
		{
			
		}

		public List<Vector2Int> LoadPeople(int count, Route route)
		{
			var listPeople = new List<Vector2Int>();
			var listDestination = new List<Vector2Int>();
			foreach (var point in route.points)
			{
				if (point != pos)
					listDestination.Add(point);
			}
			
			for (int i = 0; i < count; i++)
			{
				listPeople.Add(listDestination[Random.Range(0, listDestination.Count)]);
				peopleWait -= 1;
				if (peopleWait <= 0)
					break;
			}

			return listPeople;
		}
	}
}
