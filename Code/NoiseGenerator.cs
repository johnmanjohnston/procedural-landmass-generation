using UnityEngine;

namespace TerrainGeneration.Noise {
    public static class NoiseGenerator
    {
        public static float[,] Generate(
                uint width = 100, 
                uint height = 100, 
                float xOffset = 0f, 
                float yOffset = 0f, 
                float scale = 47f,

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
 
        // Get a Texture2D as a Material
        public static Material MapTextureToMaterial(Texture2D texture) {
            Material material = new Material(Shader.Find("Standard"));
            material.mainTexture = texture;
            material.SetFloat("_Smoothness", 0f);
            return material;
        }

        // Generate Texture2D with color depending on noise value
        public static Texture2D ColoredTexture(Texture2D orgTexture, Color[] colors, float[] values) {
            Texture2D texture = new Texture2D(orgTexture.width, orgTexture.height);
            texture.filterMode = FilterMode.Point;

            for (int x = 0; x < orgTexture.width; x++) {
                for (int y = 0; y < orgTexture.height; y++) {
                    float noiseValue = orgTexture.GetPixel(x, y).grayscale;

                    for (int i = 0; i < values.Length; i++) {
                        if (noiseValue <= values[i]) {
                            texture.SetPixel(x, y, colors[i]);
                            break;
                        }
                    }
                }
            }

            texture.Apply();
            return texture;
        }
    }
}
