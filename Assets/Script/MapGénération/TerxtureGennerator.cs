using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public static class TerxtureGennerator
{
    public static Texture2D[,] GeneratTexture(Parcel[,] parcels)
    {
        int width = parcels.GetLength(0) / 50;
        int height = parcels.GetLength(1) / 50;
        Texture2D[,] chunks = new Texture2D[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                chunks[x, y] = GenerateTextureChunck(x, y, parcels);
            }
        }
        return chunks;
    }

    public static Texture2D GenerateTextureChunck(int chunkX, int chunkY, Parcel[,] parcels)
    {
        Texture2D texture = new Texture2D(50, 50);
        Color[] colors = new Color[50 * 50];
        for (int _y = 0; _y < 50; _y++)
        {
            for (int _x = 0; _x < 50; _x++)
            {
                int x = chunkX * 50 + _x;
                int y = chunkY * 50 + _y;

                if (parcels[x, y].seeTerrain)
                {
                    Color color;
                    if (parcels[x, y].construction != null && parcels[x, y].construction.GetType() == typeof(Road))
                    {
                        color = Color.black;
                    }
                    else
                    {
                        color = Color.green;
                    }
                    colors[_y * 50 + _x] = color;
                }
            }
        }
        texture.SetPixels(colors);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
}
