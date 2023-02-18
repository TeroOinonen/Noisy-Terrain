using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TextureBuilder
{
    public static Texture2D BuildTexture(float[,] noiseMap, TerrainType[] terrainTypes)
    {
        Color[] pixels = new Color[noiseMap.Length];

        int pixelLength = noiseMap.GetLength(0);

        for (int x = 0; x < pixelLength; x++)
        {
            for (int z = 0; z < pixelLength; z++)
            {
                int index = (x * pixelLength) + z;

				foreach (TerrainType terrainType in terrainTypes)
                {
                    if (noiseMap[x, z] < terrainType.treshold)
                    {
                        pixels[index] = terrainType.color;
                        break;
                    }
                }

                //pixels[index] = Color.Lerp(Color.black, Color.white, noiseMap[x,z]);
            }
        }

        Texture2D texture = new Texture2D(pixelLength, pixelLength);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}
