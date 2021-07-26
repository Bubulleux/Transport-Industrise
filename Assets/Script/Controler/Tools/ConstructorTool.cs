using Script.Controler;
using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine;

public class ConstructorTool : Tool
{
	private Parcel.Orientation orientation;
	public ConstructorTool() : base()
	{
		Name = "Constructor";
		Modes = new[]
		{
			"Depot",
			"Loadind Bay",
			"Bus Stop",
		};
		toolColor = new Color(0.1f, 0.5f, 0.1f);
	}

	public override void OneClick(Vector2Int pos)
	{
		Parcel construction = null;

		switch (modeUsed)
		{
			case 0:
				construction = new Depot();
				break;
			case 1:
				construction = new LoadingBay();
				break;
			case 2:
				construction = new BusStop();
				break;
		}

		MapManager.map.AddConstruction(pos, construction, orientation);
	}

	public override void MidelMousseBtn()
	{
	}

	public override void MouseScrool(int scroling)
	{
		
		orientation = (Parcel.Orientation)(((int)orientation + scroling) % 4);
		if ((int) orientation < 0)
			orientation += 4;
	}

	public override void MousseOverMap(Vector2Int pos)
	{
		base.MousseOverMap(pos);
		MapManager.Selector.SelectParcel(pos + MapManager.parcelAround[(int) orientation], Color.cyan);
		
		if (modeUsed == 2)
		{
			foreach (var posArea in Helper.GetCircleArea(pos, BusStop.busStopActionRadius))
			{
				if (MapManager.map.ParcelIs<Dwelling>(posArea))
				{
					MapManager.Selector.SelectParcel(posArea, Color.blue);
				}
			}
			
			foreach (var busStop in MapManager.map.GetImpotantParcel<BusStop>())
			{
				MapManager.Selector.SelectParcel(busStop.pos, Color.red);
				foreach (var dwelling in busStop.Dwellings)
				{
					MapManager.Selector.SelectParcel(dwelling.pos, Color.white, true);
				}
			}
		}
		
		
	}
}