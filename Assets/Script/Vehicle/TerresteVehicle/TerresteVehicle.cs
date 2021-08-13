using System.Collections.Generic;
using Script.MapGeneration;
using Script.Mapping;
using Script.Mapping.ParcelType;
using Script.Vehicle.TerresteVehicle.Truck;
using UnityEngine;

namespace Script.Vehicle.TerresteVehicle
{
	public class TerresteVehicle : VehicleContoler
	{
	
		public new ParticleSystem particleSystem;

		private void Start()
		{
			particleSystem.Stop();
		}
		public override void ReturnInDepot()
		{
			vehicleStoped = true;
			if (MapManager.map.GetparcelType(VehiclePos) == typeof(Depot))
			{
				return;
			}
			path = PathFinder.FindPath(VehiclePos, GetCloserDepot().pos);
			if (path == null)
				engineEnable = false;
		}

		public override List<Vector2Int> GetPath()
		{
			return PathFinder.FindPath(VehiclePos, GetNextParcel().pos);
		}

		public virtual Parcel GetNextParcel()
		{
			if (damage > 0.8f)
			{
				Depot closerDepot = GetCloserDepot();
				if (closerDepot != null)
					return closerDepot;
			}
			RoutePointGo += 1;
			for (int i = 0; i < MyRoute.points.Count; i++)
			{
				if (MapManager.map.GetparcelType(MyRoute.points[RoutePointGo]) != typeof(LoadingBay))
					RoutePointGo += 1;
				else
					break;
			}
			return MapManager.map.GetParcel(MyRoute.points[RoutePointGo]);
		}

		public override void DoSomething()
		{
			base.DoSomething();
			if (MapManager.map.GetparcelType(VehiclePos) == typeof(LoadingBay) || MapManager.map.GetparcelType(VehiclePos) == typeof(BusStop))
			{
				float time = Load();
				SetRestart(time);
			}
		}

		public virtual float Load()
		{
			VehicleLoader loader;
			if (TryGetComponent<VehicleLoader>(out loader))
			{
				return loader.Load();
			}

			return 0f;
		}


		public override void AnimeVehicle()
		{
			if (path == null || path.Count == 0 || (path.Count == 1 && VehiclePos == path[0]))
			{
				return;
			}

			Vector2Int posBefor;
			Vector2Int posGo;
			if (VehiclePos == path[0])
			{
				posBefor = path[0] - path[1];
			}
			else
			{
				posBefor = VehiclePos - path[0];
			}
			if (path.Count == 1)
			{
				posGo = -posBefor;
			}
			else
			{
				posGo = path[1] - path[0];
			}


			float dir = Mathf.Atan2(posBefor.x * -1, posBefor.y * -1) * Mathf.Rad2Deg;
			vehicleAnimation.transform.rotation = Quaternion.Euler(0f, dir, 0f);
			int dirAnime = 0;
			if (posBefor == new Vector2Int(0, 1))
			{
				dirAnime = posGo == new Vector2Int(-1, 0) ? 1 : posGo == new Vector2Int(1, 0) ? -1 : 0;
			}
			else if (posBefor == new Vector2Int(1, 0))
			{
				dirAnime = posGo == new Vector2Int(0, 1) ? 1 : posGo == new Vector2Int(0, -1) ? -1 : 0;
			}
			else if (posBefor == new Vector2Int(0, -1))
			{
				dirAnime = posGo == new Vector2Int(1, 0) ? 1 : posGo == new Vector2Int(-1, 0) ? -1 : 0;
			}
			else if (posBefor == new Vector2Int(-1, 0))
			{
				dirAnime = posGo == new Vector2Int(0, -1) ? 1 : posGo == new Vector2Int(0, 1) ? -1 : 0;
			}
			//float dirAng = Mathf.Atan2(posGo.x, posGo.y) - Mathf.Atan2(posBefor.x, posBefor.y);
			//dirAnime = Mathf.FloorToInt(Mathf.Sin(dirAng));
			vehicleAnimation.StopPlayback();
			string animation = dirAnime == 0 ? "Forward" : dirAnime == -1 ? "TurnRight" : "TurnLeft";
			if (VehiclePos == path[0])
			{
				animation = "Start";
				particleSystem.Play();
			}
			else if (path.Count == 1)
			{
				animation = "Stop";
				particleSystem.Stop();
			}
			vehicleAnimation.Play(animation, 0, 0f);
			//vehicleAnimation.SetInteger("Direction", dirAnime);
			vehicleAnimation.SetFloat("Speed", vehicleData.speed * TimeManager.TimeScale);
		}
	}
}
