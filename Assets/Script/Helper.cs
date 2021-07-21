using UnityEngine;

namespace Script
{
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
	}
}