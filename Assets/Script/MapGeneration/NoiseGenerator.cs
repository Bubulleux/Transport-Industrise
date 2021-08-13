using UnityEngine;

namespace Script.MapGeneration
{
    public static class NoiseGenerator 
    {
        public static float[,] GenerateComplexNoise(int mapWidth, int mapHeight,float scale, int octave, float lacunarity, float persistance, int seed)
        {
            float[,] noise = new float[mapWidth, mapHeight];
            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            Vector2[] octaveOffset = new Vector2[octave];
            System.Random prng = new System.Random(seed);
            for (int i = 0; i < octave; i++)
            {
                octaveOffset[i] = new Vector2(prng.Next(-100000, 100000), prng.Next(-100000, 100000));
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {

                    float frequency = 1;
                    float amplitude = 1;
                    float noiseHeight = 0;
                    for (int i = 0; i < octave; i++)
                    {
                        float sampleX = (x + octaveOffset[i].x) / scale * frequency;
                        float sampleY = (y + octaveOffset[i].y) / scale * frequency;
                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                        noiseHeight += perlinValue * amplitude;
                        //Debug.LogFormat("Heaight: {0}, amplitude: {1}, frequancy {2}, perlin : {3}, x {4}", noiseHeight, amplitude, frequency, perlinValue, sampleX);
                        frequency *= lacunarity;
                        amplitude *= persistance;
                    }
                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }
                    noise[x, y] = noiseHeight;
                }
            }
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noise[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noise[x, y]);
                }
            }
            return noise;
        }

        public static float[,] GenerateComplexNoise(int mapWidth, int mapHeight, float scale, int octave, float lacunarity,
            float persistance, int seed, AnimationCurve curveOfHeight, AnimationCurve limitCurv)
        {
        
            var noise = GenerateComplexNoise(mapWidth, mapHeight, scale, octave, lacunarity, persistance, seed);
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int distAtLimit = GetLimitDist(new Vector2Int(x, y), new Vector2Int(mapWidth, mapHeight));
                    noise[x, y] = Mathf.Floor(curveOfHeight.Evaluate(noise[x, y]) * limitCurv.Evaluate(distAtLimit));
                    //noise[x, y] = Mathf.Floor(noise[x, y] * 6);
                }
            }

            return noise;
        }


        public static int GetLimitDist(Vector2Int pos, Vector2Int size)
        {
            int distAtLimit = pos.x;
            int[] distAteveryLimit = { pos.y, size.y - pos.y, size.x - pos.x };
            foreach (var limit in distAteveryLimit)
            {
                if (limit < distAtLimit)
                {
                    distAtLimit = limit;
                }
            }

            return distAtLimit;
        }

        public static Vector3[,] GetNormal(float[,] noise)
        {
            var normalMap = new Vector3[noise.GetLength(0) - 1, noise.GetLength(1) - 1];

            for (int y = 0; y < noise.GetLength(1) - 1; y++)
            {
                for (int x = 0; x < noise.GetLength(0) - 1; x++)
                {
                    var points = new[]
                    {
                        new Vector3(x, noise[x, y], y),
                        new Vector3(x + 1, noise[x + 1, y], y),
                        new Vector3(x + 1, noise[x + 1, y + 1], y + 1),
                        new Vector3(x, noise[x, y + 1], y+ 1),
                    };
                    var normalA = Vector3.Cross(points[1] - points[0], points[3] - points[0]).normalized;
                    var normalB = Vector3.Cross(points[3] - points[2], points[1] - points[2]).normalized;
                    normalMap[x, y] = Vector3.Cross(normalA, normalB).normalized;
                    normalMap[x, y] = normalB;
                }
            }
            
            return normalMap;
        }

        public static float[,] GenerateNoise2D(Vector2Int size, float frequancie, int seed, bool loop = false)
		{
			Vector2[,] gradients = new Vector2[Mathf.FloorToInt(size.x * frequancie) + (loop ? 0 : 1),
				Mathf.FloorToInt(size.y * frequancie + (loop ? 0 : 1))];
			
			System.Random prng = new System.Random(seed);
			for (int x = 0; x < gradients.GetLength(0); x++)
			{
				for (int y = 0; y < gradients.GetLength(1); y++)
				{
					gradients[x, y] = new Vector2(prng.Next(-100000, 100000) / 100000f,
						prng.Next(-100000, 100000) / 100000f);
					//Debug.Log(gradients[x, y]);
				}
			}

			float[,] result = new float[size.x, size.y];
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					Vector2 point = new Vector2(x * (gradients.GetLength(0) - (loop ? 0 : 1)) / (float)size.x,
						y * (gradients.GetLength(1) - (loop ? 0 : 1)) / (float)size.y);
					Vector2Int pointFloor = new Vector2Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));
					//Debug.Log($"{point}   {pointFloor} {x} {y} {gradients.GetLength(0)} {gradients.GetLength(1)}");
					Vector2Int gradientLenght = new Vector2Int(gradients.GetLength(0), gradients.GetLength(1));
					Vector2[] gradientPoint = new[]
					{
						gradients[pointFloor.x, pointFloor.y],
						gradients[(pointFloor.x + 1) % gradientLenght.x, pointFloor.y],
						gradients[(pointFloor.x + 1) % gradientLenght.x, (pointFloor.y + 1) % gradientLenght.x],
						gradients[pointFloor.x, (pointFloor.y + 1) % gradientLenght.x],
					};

					Vector2[] vectorDiff = new[]
					{
						 point - pointFloor - new Vector2(0, 0),
						 point - pointFloor - new Vector2(1, 0),
						 point - pointFloor - new Vector2(1, 1),
						 point - pointFloor - new Vector2(0, 1),
					};
					float LerpUp = Mathf.Lerp(DotProduct(gradientPoint[0], vectorDiff[0]),
						Vector2.Dot(gradientPoint[1], vectorDiff[1]),   Fade(point.x - pointFloor.x));
					
					float LerpDown = Mathf.Lerp(DotProduct(gradientPoint[3], vectorDiff[3]),
						Vector2.Dot(gradientPoint[2], vectorDiff[2]), Fade(point.x  - pointFloor.x));
					result[x, y] = (Mathf.Lerp(LerpUp, LerpDown, Fade(point.y  - pointFloor.y)) + 1) / 2f;
					//result[x, y] = gradients[pointFloor.x, pointFloor.y].x + 1 /2;
				}
			}
			return result;
		}
        
        public static float DotProduct(Vector2 posA, Vector2 posB)
        {
        	return posA.x * posB.x + posA.y * posB.y;
        }

        public static float Fade(float t)
        {
        	return Mathf.Pow(t, 3f) * (t * (t * 6 - 15) + 10);
        }
    }
}
