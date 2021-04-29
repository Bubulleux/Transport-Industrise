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
                Vector2Int pos = new Vector2Int(x, y);
                if (!map.GetParcel(pos).seeTerrain)
                    Debug.Log("Dont See Terrain");
                colors[_y * 50 + _x] = map.GetParcel(pos).seeTerrain?  (Color)map.GetParcel(pos).color : new Color(255, 0, 255);
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
