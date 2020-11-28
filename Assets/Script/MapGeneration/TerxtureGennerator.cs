using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
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
                //chunks[x, y] = GenerateTextureChunck(x, y, parcels);
            }
        }
        return chunks;
    }

    public static async Task AsyncGenerateTextureChunck(int chunkX, int chunkY, Parcel[,] parcels, GameObject chunckGo)
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
                    if (parcels[x, y].GetType() == typeof(Road))
                    {
                        color = Color.black;
                    }
                    else if (parcels[x, y].GetType() == typeof(Depot))
                    {
                        color = new Color(0.7f, 0.1f, 0f);
                    }
                    else if (parcels[x, y].GetType() == typeof(Building))
                    {
                        color = ((Building)parcels[x, y]).color;
                    }
                    else if (parcels[x, y].GetType() == typeof(LoadingBay))
                    {
                        color = Color.white;
                    }
                    else
                    {
                        color = Color.green;
                    }
                    colors[_y * 50 + _x] = color;
                    
                }
            }
        }
        await Task.Delay(1);
        texture.SetPixels(colors);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        chunckGo.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        chunckGo.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
    }
}
