using System.Collections.Generic;
using Script.Mapping;
using UnityEngine;


public class RoadTool : Tool
{
	private int minSizeRoad = 50;
	public RoadTool() : base()
	{
		Name = "Road Creator";
		toolColor = Color.black;
	}
	
	public override void OneClick(Vector2Int pos)
	{
		MapManager.map.AddRoad(pos);
	}

	

	public override void Drag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
		foreach (var pathCell in GetPathBetweenTwoPoint(start, stop, minSizeRoad))
		{
			MapManager.Selector.SelectParcel(pathCell.Key, Color.black);
		}
	}

	public override void StopDrag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
		foreach (var pathCell in GetPathBetweenTwoPoint(start, stop, minSizeRoad))
		{
			MapManager.map.AddRoad(pathCell.Key);
		}
	}

	public override void CancelDrag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
	}

	

	public override void MouseScrool(int scroling)
	{
		minSizeRoad += scroling * 2;
		if (minSizeRoad > 100)
			minSizeRoad = 100;
		if (minSizeRoad < 0)
			minSizeRoad = 0;
	}

	public static Dictionary<Vector2Int, bool> GetPathBetweenTwoPoint(Vector2Int start, Vector2Int stop, int minSizeRoad)
	{
		
		Vector2Int lastPos = start;
		var path = new Dictionary<Vector2Int, bool>();
		path.Add(start, true);
		var XAxe = false;
		if (minSizeRoad < 0)
			return null;
		var whileI = 0;
		while (true)
		{
			if (lastPos == stop)
			{
				break;
			}
			float xProgress = Mathf.Abs(lastPos.x - start.x) / (float)Mathf.Abs(start.x - stop.x);
			float yProgress = Mathf.Abs(lastPos.y - start.y) / (float)Mathf.Abs(start.y - stop.y);
			xProgress = (float.IsNaN(xProgress) || float.IsInfinity(xProgress)) ? 1 : xProgress;
			yProgress = (float.IsNaN(yProgress) || float.IsInfinity(yProgress)) ? 1 : yProgress;

			if (XAxe ? yProgress < xProgress : xProgress < yProgress)
				XAxe = !XAxe;
			for (int i = 0; i <= minSizeRoad; i++)
			{
				lastPos += new Vector2Int(XAxe ? (lastPos.x < stop.x ? 1 : -1) : 0,
					!XAxe ? (lastPos.y < stop.y ? 1 : -1) : 0);
				path.Add(lastPos, true);
				
				if ((XAxe ? lastPos.x : lastPos.y) == (XAxe ? stop.x : stop.y)) { break; }
			}

			whileI += 1;
			if (whileI >= 1000)
				break;

		}
		
		return path;
	}
	
}