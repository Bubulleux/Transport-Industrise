using System;
using Script.MapGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
	public class Test : MonoBehaviour
	{
		public Renderer renderer;
		public Vector2Int size = new Vector2Int(100, 100);
		public float InputFreq = 1;
		public int seed = 4269;
		public bool PrintTest(bool value, string text)
		{
			Debug.Log(text);
			return value;
		}

		private void Start()
		{
			if (PrintTest(true, "First") || PrintTest(false, "First"))
			{
				Debug.Log("OK");
			}
			
			Debug.Log(Mathf.Lerp(0, 100, 0.3f));
			
			

		}

		public void Update()
		{
			// if (renderer == null)
			// 	return;
			//
			// //var noise = NoiseGenerator.GenerNoise(1001, 1001, 100, 3, 6, 0.1f, 100000);
			// var noise = NoiseGenerator.GenerateNoise2D(size, InputFreq, seed);
			// //var normalNoise = NoiseGenerator.GetNormal(noise);
			// Texture2D texture = new Texture2D(size.x , size.y);
			// Color[] colors = new Color[size.x * size.y];
			// float sum = 0;
			// for (int _y = 0; _y < size.y; _y++)
			// {
			// 	for (int _x = 0; _x < size.x; _x++)
			// 	{
			// 		colors[_y  * size.x + _x] = Color.white * noise[_x, _y];
			// 		sum += noise[_x, _y];
			// 		//Vector3 vector = normalNoise[_x, _y];
			// 		//Debug.Log($"pos {_x},{_y}, {vector.x} {vector.y} {vector.z}");
			// 		//colors[_y * 10 + _x] = new Color(vector.x, vector.y, vector.z) * noise[_x, _y];
			// 	}
			// }
			//
			// Debug.Log($"Mean {sum / (size.x * size.y)}");
			// texture.SetPixels(colors);
			// texture.filterMode = FilterMode.Point;
			// texture.wrapMode = TextureWrapMode.Clamp;
			// texture.Apply();
			// renderer.material.mainTexture = texture;
		}

		
		
		// public static float[] GenerateNoiseXD(int[] size, float frequancie, int seed)
		// {
		// 	System.Random prng = new System.Random(seed);
		// 	
		// 	int[] gradientDimentionSize = new int[size.Length];
		// 	int cellCount = 1;
		// 	int childCellCount = 1;
		// 	for (int i = 0; i < size.Length; i++)
		// 	{
		// 		gradientDimentionSize[i] = Mathf.FloorToInt(size[i] * frequancie) + 1;
		// 		cellCount *= gradientDimentionSize[i];
		// 		if (i != 0)
		// 			childCellCount *= gradientDimentionSize[i];
		// 	}
		//
		// 	Vector2[] gradients = GetRandomGradient(gradientDimentionSize, prng.Next(1, 100000));
		//
		// 	float[] result = new float[ArrayProduct(size)];
		// 	for (int x = 0; x < size.x; x++)
		// 	{
		// 		for (int y = 0; y < size.y; y++)
		// 		{
		// 			Vector2 point = new Vector2(x * (gradients.GetLength(0) - 1) / (float)size.x,
		// 				y * (gradients.GetLength(1) - 1) / (float)size.y);
		// 			Vector2Int pointFloor = new Vector2Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));
		// 			//Debug.Log($"{point}   {pointFloor} {x} {y} {gradients.GetLength(0)} {gradients.GetLength(1)}");
		// 			Vector2[] gradientPoint = new[]
		// 			{
		// 				gradients[pointFloor.x, pointFloor.y],
		// 				gradients[pointFloor.x + 1, pointFloor.y],
		// 				gradients[pointFloor.x + 1, pointFloor.y + 1],
		// 				gradients[pointFloor.x, pointFloor.y + 1],
		// 			};
		//
		// 			Vector2[] vectorDiff = new[]
		// 			{
		// 				 point - pointFloor - new Vector2(0, 0),
		// 				 point - pointFloor - new Vector2(1, 0),
		// 				 point - pointFloor - new Vector2(1, 1),
		// 				 point - pointFloor - new Vector2(0, 1),
		// 			};
		// 			float LerpUp = Mathf.Lerp(DotProduct(gradientPoint[0], vectorDiff[0]),
		// 				Vector2.Dot(gradientPoint[1], vectorDiff[1]),   Fade(point.x - pointFloor.x));
		// 			
		// 			float LerpDown = Mathf.Lerp(DotProduct(gradientPoint[3], vectorDiff[3]),
		// 				Vector2.Dot(gradientPoint[2], vectorDiff[2]), Fade(point.x  - pointFloor.x));
		// 			result[x, y] = (Mathf.Lerp(LerpUp, LerpDown, Fade(point.y  - pointFloor.y)) + 1) / 2f;
		// 			//result[x, y] = gradients[pointFloor.x, pointFloor.y].x + 1 /2;
		// 		}
		// 	}
		// 	return result;
		//
		// }
		//
		// public static Vector2[] GetRandomGradient(int[] size, int seed)
		// {
		// 	var result = new Vector2[ArrayProduct(size)];
		// 	for (int i = 0; i < size[0]; i++)
		// 	{
		// 		System.Random prng = new System.Random(seed);
		// 		if (size.Length == 1)
		// 		{
		// 			result[i] = new Vector2(prng.Next(-100000, 100000) / 100000f,
		// 				prng.Next(-100000, 100000) / 100000f);
		// 		}
		// 		else
		// 		{
		// 			Vector2[] gradient = GetRandomGradient(Remove(size, 0), prng.Next(1, 100000));
		// 			for (int j = 0; j < gradient.Length; j++)
		// 			{
		// 				result[i * gradient.Length + j] = gradient[j];
		// 			}
		// 		}
		// 	}
		//
		// 	return result;
		// }
		//
		// public static float[] GetNoiseByGradient(int[] size, int[] sizeGradient, int[] pos, float[,] gradients)
		// {
		// 	for (int i = 0; i < size[i]; i++)
		// 	{
		// 		if (size.Length == 1)
		// 		{
		// 			
		// 		}
		// 	}
		// }
		//
		// public static float[] GetValueByPos(float[] pos, int[] posFloor, int dimention, float[,] gradient, int[] sizeGradient)
		// {
		// 	if (dimention == pos.Length - 1)
		// 	{
		// 		
		// 	}
		// }
		//
		// public static float DotProductXD(float[] pos, int[] gradientPos, float[,] gradient, int[] gradientSize)
		// {
		// 	float[] diff = new float[pos.Length];
		// 	for (int i = 0; i < pos.Length; i++)
		// 	{
		// 		diff[i] = pos[i] - gradientPos[i];
		// 	}
		//
		// 	float[] gradientValue = float[gradientSize.Length];
		// }
	}
}


