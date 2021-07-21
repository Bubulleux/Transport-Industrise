using System.Runtime.CompilerServices;
using Script.Mapping;
using UnityEngine;


public class Tool
{
	public string Name { get; set; }
	public string[] Modes { get; set; }
	public int modeUsed = 0;
	public Color toolColor;

	public Tool()
	{
		Name = "Interact";
		toolColor = Color.blue;
	}

	public virtual void OneClick(Vector2Int pos)
	{
		MapManager.map.GetParcel(pos).Interact();
	}

	public virtual void Drag(Vector2Int start, Vector2Int stop) { }
	public virtual void StopDrag(Vector2Int start, Vector2Int stop) { }
	public virtual void CancelDrag(Vector2Int start, Vector2Int stop) { }

	public virtual void MousseOverMap(Vector2Int pos) 
	{ 
		MapManager.Selector.ClearSelection();
		MapManager.Selector.SelectionParcel(pos, toolColor);
		
	}
	
	public virtual void StartUsing() { }
	public virtual void StopUsing() { }
	
}
