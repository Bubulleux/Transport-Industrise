using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine;

public class ConstructorTool : Tool
{
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

		MapManager.map.AddConstruction(pos, construction);
	}

}