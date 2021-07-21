using System.Collections.Generic;
using Script.Mapping;
using UnityEngine;


public class RoadTool : Tool
{

	public RoadTool() : base()
	{
		Name = "Road Creator";
		Modes = new[]
		{
			"A",
			"B",
		};
		toolColor = Color.black;
	}
	
	public override void OneClick(Vector2Int pos)
	{
		MapManager.map.AddRoad(pos);
	}

	

	public override void Drag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
		foreach (var pathCell in GetPathBetweenTwoPoint(start, stop, 50))
		{
			MapManager.Selector.SelectionParcel(pathCell.Key, Color.black);
		}
	}

	public override void StopDrag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
		foreach (var pathCell in GetPathBetweenTwoPoint(start, stop, 50))
		{
			MapManager.map.AddRoad(pathCell.Key);
		}
	}

	public override void CancelDrag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
	}
	
	public static Dictionary<Vector2Int, bool> GetPathBetweenTwoPoint(Vector2Int start, Vector2Int stop, int minSizeRoad)
	{
		
		Vector2Int lastPos = start;
		var path = new Dictionary<Vector2Int, bool>();
		path.Add(start, true);
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
			if (xProgress < yProgress)
			{
				for (int i = 0; i < minSizeRoad; i++)
				{
					lastPos += new Vector2Int(lastPos.x < stop.x ? 1 : -1, 0);
					path.Add(lastPos, true);
					if (lastPos.x == stop.x) { break; }
				}
			}
			else
			{
				for (int i = 0; i < minSizeRoad; i++)
				{
					lastPos += new Vector2Int(0, lastPos.y < stop.y ? 1 : -1);
					path.Add(lastPos, true);
					if (lastPos.y == stop.y) { break; }
				}
			}
	   
		}
		
		return path;
	}
	
}