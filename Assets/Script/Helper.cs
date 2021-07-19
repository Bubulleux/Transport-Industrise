using UnityEngine;
using System;


public static class Helper
{
	public static Vector2Int ToVec2Int(this Vector3 vec)
    {
    	return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.z));
    }
}