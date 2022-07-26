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

        private float[,] noise;
        private Texture2D texture;

        private MeshFilter mFilter; 
        private MeshRenderer mRenderer;

        private void Start() {
            terrainGameObject = GameObject.FindGameObjectWithTag("Mesh");
            noiseTextureUI = GameObject.Find("NoiseTexture");
            
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

            SetUINoiseTexture();
        }

        private void SetUINoiseTexture() {
            Image image = noiseTextureUI.GetComponent<Image>();

            image.sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            image.preserveAspect = true;
        }

        private Vector3[] vertices;
        private void GenerateMesh() {
            noise = NoiseGenerator.Generate(
                 width,
                 height,
                 xOffset,
                 yOffset,
                 scale
            );

            MeshGenerator meshGenerator = new MeshGenerator(
                width,
                height,
                texture,
                heightMultiplier
            );

            terrainGameObject.GetComponent<MeshFilter>().mesh = meshGenerator.UpdatedMesh();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (vertices == null) return;

            Gizmos.color = Color.white;

            for (int i = 0; i < vertices.Length; i++) {
                Gizmos.DrawCube(vertices[i], Vector3.one);
            }
        }
        #endif
    }
}
