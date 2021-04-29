using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : Road
{
	public const busStopActionRadius;
	public int peopleWait = 0;

	public List<Dwelling> Dwellings { get
		{
			List<Dwelling> _dwellings = new List<Dwelling>();
			foreach(Dwelling curDwelling in  MapManager.map.GetImpotantParcel<Dwelling>())
			{
				if (Vector2Int.Distance(curDwelling.pos, pos) < )
			}
		} 
	}
}
