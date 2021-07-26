using System.Collections.Generic;
using UnityEngine;


public static class Helper
{
	public static Vector2Int ToVec2Int(this Vector3 vec)
	{
		return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.z));
	}

	public static Color ColorSetAlpha(this Color color, float alpha)
	{
		return new Color(color.r, color.b, color.g, alpha);
	}

	public static List<Vector2Int> GetArea(Vector2Int posA, Vector2Int posB, bool aroundOnly = false)
	{
		var origin = new Vector2Int(posA.x > posB.x ? posB.x : posA.x, posA.y > posB.y ? posB.y : posA.y);
		var point = new Vector2Int(posA.x < posB.x ? posB.x : posA.x, posA.y < posB.y ? posB.y : posA.y);
		var area = new List<Vector2Int>();
		for (var y = origin.y; y <= point.y; y++)
		{
			for (var x = origin.x; x <= point.x; x++)
			{
				if (aroundOnly && x != origin.x && y != origin.y && y != point.y && x != point.x )
					continue;
				area.Add(new Vector2Int(x, y));
			}
		}

		return area;
	}

	public static List<Vector2Int> GetCircleArea(Vector2Int origin, int radius)
	{
		var area = new List<Vector2Int>();
		for (int y = -radius; y <= radius; y++)
		{
			for (int x = -radius; x <= radius ; x++)
			{
				var pos = new Vector2Int(x, y);
				if (Vector2Int.Distance(pos, Vector2Int.zero) <= radius)
				{
					area.Add(pos + origin);
				}
			}
		}
		return area;
	}
}