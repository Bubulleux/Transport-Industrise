using System;
using System.Collections.Generic;
using Script.Game;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
	public class BusStop : LoadingBay
	{
		public const int busStopActionRadius = 4;
		private float peopleWait = 0f;
		public int PeopleWait { get { return Mathf.FloorToInt(peopleWait); } set { peopleWait = value; } }
		public static MaterialData PeopleMatarial { get { return Resources.Load("ScriptableObject/Materials/People") as MaterialData; } }
		public const int maxPeopleWaiting = 100;


		public List<Dwelling> Dwellings { get
			{
				var dwellings = new List<Dwelling>();

				foreach (var posArea in Helper.GetCircleArea(pos, busStopActionRadius))
				{
					if (map.ParcelIs<Dwelling>(posArea))
						dwellings.Add(map.GetParcel<Dwelling>(posArea));
				}
				return dwellings;
			} 
		}

		public override bool CanUnload(MaterialData material)
		{
			return material == PeopleMatarial;
		}

		public override int TryToInteract(MaterialData material, int materialQuantityGive)
		{
			Debug.Log("TryToInteract: " + materialQuantityGive);
			if (material != PeopleMatarial)
				return 0;
			if (materialQuantityGive > 0)
				return materialQuantityGive;

			if (-materialQuantityGive > PeopleWait)
			{
				GameManager.Money -= PeopleMatarial.buyPrice * PeopleWait;
				PeopleWait = 0;
				return PeopleWait;
			}
			else
			{
				GameManager.Money -= PeopleMatarial.buyPrice * materialQuantityGive;
				PeopleWait += materialQuantityGive;
				return materialQuantityGive;
			}
		}

		public override Dictionary<MaterialData, MaterialInfo> GetMaterial(bool getInput)
		{
			return new Dictionary<MaterialData, MaterialInfo>() { { PeopleMatarial, new MaterialInfo() { 
				isInput = getInput,
				maxQuantity = getInput ? 9999 : maxPeopleWaiting,
				quantity = getInput ? 0 : PeopleWait,
			} } };
		}

		public override void InitializationSecondary()
		{
			color = Color.white;
			EnableUpdate(true);
			prefab = Resources.Load<GameObject>("ParcelGFX/BusStop");
		}

		public override void Update()
		{
			if (peopleWait < maxPeopleWaiting)
			{
				foreach (Dwelling curDwelling in Dwellings)
				{
					peopleWait += curDwelling.dwell / 100f * Time.deltaTime;
				}
			}
			else
				peopleWait = maxPeopleWaiting;
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
	}
}
