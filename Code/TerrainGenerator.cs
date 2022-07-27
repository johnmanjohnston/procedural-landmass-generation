using UnityEngine;
using UnityEngine.UI;
using TerrainGeneration.Noise;
using TerrainGeneration.MeshGeneration;
using System.Collections;

namespace TerrainGeneration.Generation {
    public class TerrainGenerator : MonoBehaviour
    {
        private GameObject terrainGameObject;
        private GameObject noiseTextureUI;
        private GameObject coloredTextureUI;

        [Header("Noise Parameters")]
        [Space]
        [SerializeField] private uint width;
        [SerializeField] private uint height;
        [Space]
        [SerializeField] private float xOffset;
        [SerializeField] private float yOffset;
        [Space]
        [SerializeField] private float scale;
        [SerializeField] private float heightMultiplier;

        [SerializeField] private uint octaves;

        [Header("Color Specifications")]
        [Space]
        [SerializeField] private Color[] colors;
        [SerializeField] private float[] colorHeights;

        [Header("Simulation Specifications")]
        [SerializeField] private bool renderColors;


        private float[,] noise;
        private Texture2D texture;
        private Texture2D coloredTexture;

        private Renderer terrainRenderer;
        private MeshFilter terrainMeshFilter;

        private void Start() {
            terrainGameObject = GameObject.FindGameObjectWithTag("Mesh");

            noiseTextureUI = GameObject.Find("NoiseTexture");
            coloredTextureUI = GameObject.Find("ColoredTexture");

            terrainRenderer = terrainGameObject.GetComponent<Renderer>();
            terrainMeshFilter = terrainRenderer.GetComponent<MeshFilter>();
            
            StartCoroutine(Init());
        }

        private IEnumerator Init() {
            GenerateNoise();
            GenerateMesh();

            yield return new WaitForSeconds(0.05f);
            StartCoroutine(Init());
        }

        private void GenerateNoise() {
            yOffset += 0.07f;
            xOffset += 0.07f;

            noise = NoiseGenerator.Generate(
                 width,
                 height,
                 xOffset,
                 yOffset,
                 scale,
                 octaves
            );

            texture = NoiseGenerator.NoiseTexture(noise);

            Material material;

            if (renderColors) {
                coloredTexture = NoiseGenerator.ColoredTexture(texture, colors, colorHeights);
                material = NoiseGenerator.MapTextureToMaterial(coloredTexture);
            } else {
                material = NoiseGenerator.MapTextureToMaterial(texture);
            }

            terrainRenderer.material = material;

            UpdateTexturePreviews();
        }

        private void UpdateTexturePreviews() {
            // Update the noise texture preview
            Image noiseImg = noiseTextureUI.GetComponent<Image>();

            noiseImg.sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            noiseImg.preserveAspect = true;

            // then, the colored texture preview
            Image coloredImg = coloredTextureUI.GetComponent<Image>();

            coloredImg.sprite = Sprite.Create(
                coloredTexture,
                new Rect(0f, 0f, coloredTexture.width, coloredTexture.height),
                new Vector2(0.5f, 0.5f)
            );

            coloredImg.preserveAspect = true;
        }

        private void GenerateMesh() {
            noise = NoiseGenerator.Generate(
                 width,
                 height,
                 xOffset,
                 yOffset,
                 scale
            );

            MeshGenerator meshGenerator = new MeshGenerator(
                texture,
                heightMultiplier
            );

            terrainMeshFilter.mesh = meshGenerator.UpdatedMesh();
        }
    }
}
