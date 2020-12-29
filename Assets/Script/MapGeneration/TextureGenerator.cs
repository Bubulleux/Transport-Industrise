using System.Threading.Tasks;
using UnityEngine;
public static class TextureGenerator
{
    public static async Task AsyncGenerateTextureChunk(Vector2Int chunk, Map map, GameObject chunkGo)
    {
        chunkGo.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Standard")) { mainTexture = await AsyncGetChunkTexture(chunk, map) };
    }

    public static async Task<Texture2D> AsyncGetChunkTexture(Vector2Int chunk, Map map)
    {
        Texture2D texture = new Texture2D(50, 50);
        Color[] colors = new Color[50 * 50];
        for (int _y = 0; _y < 50; _y++)
        {
            for (int _x = 0; _x < 50; _x++)
            {
                int x = chunk.x * 50 + _x;
                int y = chunk.y * 50 + _y;

                if (map.parcels[x, y].seeTerrain)
                {
                    Color color;
                    if (map.parcels[x, y].GetType() == typeof(Road))
                    {
                        color = Color.black;
                    }
                    else if (map.parcels[x, y].GetType() == typeof(Depot))
                    {
                        color = new Color(0.7f, 0.1f, 0f);
                    }
                    else if (map.parcels[x, y].GetType() == typeof(Building))
                    {
                        color = ((Building)map.parcels[x, y]).color;
                    }
                    else if (map.parcels[x, y].GetType() == typeof(LoadingBay))
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
        await AsyncTask.DelayIfNeed(1);
        texture.SetPixels(colors);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }

}
