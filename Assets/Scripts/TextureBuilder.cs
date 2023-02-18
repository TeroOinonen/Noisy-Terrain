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

				for (int t = 0; t < terrainTypes.Length; t++)
                {
                    if (noiseMap[x, z] < terrainTypes[t].treshold)
                    {
                        float minVal = t ==0 ? 0 : terrainTypes[t-1].treshold;
                        float maxVal = terrainTypes[t].treshold;

                        pixels[index] = terrainTypes[t].colorGradient.Evaluate(1f - (maxVal - noiseMap[x,z]) / (maxVal - minVal));
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
