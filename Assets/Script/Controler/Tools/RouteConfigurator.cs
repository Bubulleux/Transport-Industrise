
using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine;

public class RouteConfigurator : Tool
{
	private Route route;
	private RouteCreatorWindow window;
	public RouteConfigurator(Route _route, RouteCreatorWindow _window) : base()
	{
		route = _route;
		window = _window;
		toolColor = Color.gray;
	}

	public override void MousseOverMap(Vector2Int pos)
	{
		base.MousseOverMap(pos);
		foreach (var point in route.points)
		{
			MapManager.Selector.SelectParcel(point, Color.white);
		}
	}

	public override void OneClick(Vector2Int pos)
	{
		if (MapManager.map.ParcelIs<Road>(pos))
		{
			if (route.points.Contains(pos))
			{
				route.points.Remove(pos);
				window.UpdateList();
			}
			else
			{
				route.points.Add(pos);
				window.UpdateList();
			}
		}
	}
}