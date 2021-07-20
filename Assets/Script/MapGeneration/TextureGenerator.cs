using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine;

namespace Script.MapGeneration
{
    public static class TextureGenerator
    {
        public static async void GenerateTextureChunk(Vector2Int chunk, Map map, GameObject chunkGo)
        {
            chunkGo.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Standard")) { mainTexture = GetChunkTexture(chunk, map) };
        }

        public static Texture2D GetChunkTexture(Vector2Int chunk, Map map)
        {
            Texture2D texture = new Texture2D(Map.ChuckSize , Map.ChuckSize);
            Color[] colors = new Color[Map.ChuckSize * Map.ChuckSize];
        
            for (int _y = 0; _y < Map.ChuckSize; _y++)
            {
                for (int _x = 0; _x < Map.ChuckSize; _x++)
                {
                    int x = chunk.x * Map.ChuckSize + _x;
                    int y = chunk.y * Map.ChuckSize + _y;
                    Vector2Int pos = new Vector2Int(x, y);
                    Parcel parcel = map.GetParcel(pos);
                    colors[_y  * Map.ChuckSize + _x] = parcel.seeTerrain ?  (Color)parcel.color : new Color(255, 0, 255);
                
                }
            }
            texture.SetPixels(colors);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }

    }
}
