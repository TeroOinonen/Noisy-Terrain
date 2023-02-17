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

                TerrainType terrainType1(float level) =>
                    noiseMap[x, z] switch
                    {


				//string WaterState(int tempInFahrenheit) =>
	   //         tempInFahrenheit switch
	   //         {
		  //          (> 32) and (< 212) => "liquid",
		  //          < 32 => "solid",
		  //          > 212 => "gas",
		  //          32 => "solid/liquid transition",
		  //          212 => "liquid / gas transition",
	   //         };

				foreach (TerrainType terrainType in terrainTypes)
                {

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
