
using Script.Mapping;
using UnityEngine;

public class Terraformer: Tool
{
	public Terraformer()
	{
		Name = "TerraFormer";
		Modes = new []
		{
			"Ascend",
			"Desend",
			"Smoothing",
		};
		toolColor = Color.white;
	}

	public override void OneClick(Vector2Int pos)
	{
		TerraForm(pos);
	}

	public override void Drag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
		MapManager.Selector.SelectArea(start, stop, toolColor);
	}

	public override void StopDrag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
		foreach (var pos in Helper.GetArea(start, stop))
		{
			TerraForm(pos);
		}
	}

	public override void CancelDrag(Vector2Int start, Vector2Int stop)
	{
		MapManager.Selector.ClearSelection();
	}

	private void TerraForm(Vector2Int pos)
	{
		var futurCorner = new int[4];
		var corner = MapManager.map.GetParcel(pos).corner;
		switch (modeUsed)
		{
			case 0:
				for (int i = 0; i < 4; i++)
					futurCorner[i] = corner[i] + 1;
				break;
			case 1:
				for (int i = 0; i < 4; i++)
					futurCorner[i] = corner[i] - 1;
				break;
			case 2:
				break;
		}
		
		MapManager.map.SetParcelCorner(pos, futurCorner);
	}
}