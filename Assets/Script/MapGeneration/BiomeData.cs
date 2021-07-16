using UnityEngine;

namespace Script.MapGeneration
{
	[CreateAssetMenu(fileName = "Biome", menuName = "MyGame/Biome", order = 0)]
	public class BiomeData : ScriptableObject
	{
		public new string name = "New Biome";
		public Color color = Color.magenta;
		public float coef = 1f;

	}
}