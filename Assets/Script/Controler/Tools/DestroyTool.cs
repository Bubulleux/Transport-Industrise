using Script.Mapping;
using  Script.Mapping.ParcelType;
using UnityEngine;


public class DestroyTool : Tool
{
	public DestroyTool()
	{
		Name = "Destoryer";
		toolColor = Color.red;
	}

	public override void OneClick(Vector2Int pos)
	{
		MapManager.map.Destroy(pos);
	}

	public override void Drag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
		MapManager.Selector.SelectArea(start, stop, Color.red);
	}

	public override void StopDrag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
		foreach (var pos in Helper.GetArea(start, stop))
		{
			MapManager.map.Destroy(pos);
		}
	}

	public override void CancelDrag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
	}
}
