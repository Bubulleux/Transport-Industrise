using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerxtureGennerator
{
    public enum TexturRenderType
    {
        blackAndWhite,
        hsv,
        region
    }
    public static Texture2D GenerTexture(float[,] noise, TexturRenderType typeOfRender, region[] regions)
    {
        int width = noise.GetLength(0);
        int height = noise.GetLength(1);

        Color[] colors = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (typeOfRender == TexturRenderType.blackAndWhite)
                {
                    colors[y * width + x] = Color.Lerp(Color.black, Color.white, noise[x, y]);
                }
                else if (typeOfRender == TexturRenderType.hsv)
                {
                    colors[y * width + x] = Color.HSVToRGB(noise[x, y], 1f, 1f);
                }
                else if (typeOfRender == TexturRenderType.region)
                {
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if(noise[x, y] <= regions[i].maxHeight)
                        {
                            colors[y * width + x] = regions[i].color;
                            break;
                        }
                    }
                }
            }
        }
        Texture2D texture = new Texture2D(width, height);

        //Debug.Log(colors[1] + "  " + colors[100]);
        texture.SetPixels(colors);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
    [System.Serializable]
    public  struct region
    {
        public string name;
        public float maxHeight;
        public Color color;
    }
}
