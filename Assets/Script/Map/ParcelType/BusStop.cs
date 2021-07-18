using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : LoadingBay
{
	public const int busStopActionRadius = 10;
	private float peopleWait = 0f;
	public int PeopleWait { get { return Mathf.FloorToInt(peopleWait); } set { peopleWait = value; } }
	public static MaterialData PeopleMatarial { get { return Resources.Load("ScriptableObject/Materials/People") as MaterialData; } }
	public const int maxPeopleWaiting = 100;


	public List<Dwelling> Dwellings { get
		{
			List<Dwelling> _dwellings = new List<Dwelling>();
			for (int y = busStopActionRadius / -2; y < busStopActionRadius / 2; y++)
			{
				for (int x = busStopActionRadius / -2; x < busStopActionRadius / 2; x++)
				{
					Vector2Int _pos = new Vector2Int(x, y);
					if (Vector2Int.Distance(_pos, Vector2Int.zero) < busStopActionRadius && MapManager.map.GetparcelType(_pos + pos) == typeof(Dwelling))
					{
						_dwellings.Add(MapManager.map.GetParcel<Dwelling>(_pos + pos));
					}
				}
			}
			return _dwellings;
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

	public override void Initialaze()
	{
		color = Color.white;
		EnableUpdate(true);
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
}
