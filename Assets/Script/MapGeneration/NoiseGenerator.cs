using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator 
{
    public static float[,] GenerNoise(int mapWidth, int mapHeight,float scale, int octave, float lacunarity, float persistance, int seed, AnimationCurve curveOfHeight, AnimationCurve limitCurv)
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
                int distAtLimit = GetLimitDist(new Vector2Int(x, y), new Vector2Int(mapWidth, mapHeight));
                noise[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noise[x, y]);
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
}
