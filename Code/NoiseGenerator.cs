using UnityEngine;
using System;

namespace TerrainGeneration.Noise {
    public static class NoiseGenerator
    {
        // Generate noise as float array
        public static float[,] Generate(
                uint width, 
                uint height, 
                float xOffset, 
                float yOffset, 
                float scale,

                uint octaves = 8
            ) {
            
            float[,] noise = new float[width, height];

            // First, check if the arguments can be used to properly generate the noise
            if (width == 0 || height == 0) {
                Debug.LogError("Invalid width/height for noise generation");
                return new float[0, 0];
            }

            if (scale == 0f) {
                Debug.LogError("Invalid scale for noise generation");
                return new float[0, 0];
            }

            if (octaves <= 0) {
                Debug.LogError("Invalid octave count for noise generation");
                return new float[0, 0];
            }

            scale += 1f;

            // Generate actual noise, as we've validated the arguments
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {

                    float totalValue = 0f;

                    for (int i = 1; i <= octaves; i++) {
                        float xValue = x / scale * (i / 1.25f);
                        float yValue = y / scale * (i / 1.25f);

                        float noiseValue = Mathf.PerlinNoise(xValue + xOffset, yValue + yOffset);
                        totalValue += noiseValue / ((i * i) + i); // Creates a desirable noise effect, modify this equation for different results
                    }

                    noise[x, y] = totalValue;
                }
            }

            return noise;
        }

        // Get noise as Texture2D
        public static Texture2D NoiseTexture(float[,] noise) {
            Texture2D texture = new Texture2D(noise.GetLength(0), noise.GetLength(1));
            texture.filterMode = FilterMode.Point;

            for (int x = 0; x < noise.GetLength(0); x++) {
                for (int y = 0; y < noise.GetLength(1); y++) {
                    float noiseValue = noise[x, y];
                    texture.SetPixel(x, y, new Color(noiseValue, noiseValue, noiseValue));
                }
            }

            texture.Apply();
            return texture;
        }
 
        // Get noise as Material
        public static Material NoiseMaterial(Texture2D noiseTexture) {
            Material material = new Material(Shader.Find("Standard"));
            material.mainTexture = noiseTexture;
            return material;
        }
    }
}
