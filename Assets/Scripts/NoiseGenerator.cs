using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public static float[,] GenerateNoiseMap(int noiseSampleSize, float scale, Wave[] waves, int resolution = 1)
    {
        float[,] noiseMap = new float[noiseSampleSize * resolution, noiseSampleSize * resolution];

        for (int x = 0; x < noiseSampleSize * resolution; x++)
        {
            for (int y = 0; y < noiseSampleSize * resolution; y++)
            {
                float samplePosX = (float)x / scale / (float)resolution;
				float samplePosY = (float)y / scale / (float)resolution;

                // Pure perlin noise
                //noiseMap[x, y] = Mathf.PerlinNoise(samplePosX, samplePosY);

                // Wave modified noise
                float noise = 0f;
                float normalization = 0f;

                foreach (Wave wave in waves)
                {
                    noise += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency + wave.seed, samplePosY * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
				}

                noise /= normalization;
				noiseMap[x, y] = noise;
			}
        }


        return noiseMap;
    }
}
