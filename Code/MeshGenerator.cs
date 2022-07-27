using UnityEngine;

namespace TerrainGeneration.MeshGeneration {
    public class MeshGenerator
    {
        private readonly int xSize;
        private readonly int ySize;
        private readonly Texture2D noiseTexture;
        private readonly float heightMultiplier;

        private Mesh mesh;

        public Vector3[] GenerateVertices() {
            Vector3[] vertices = new Vector3[(xSize + 1) * (ySize + 1)];

            for (int z = 0, i = 0; z <= ySize; z++) {
                for (int x = 0; x <= xSize; x++) {
                    float y = noiseTexture.GetPixel(x, z).grayscale * heightMultiplier;
                    vertices[i] = new Vector3(x, y, z);
                    i++;
                }
            }

            return vertices;
        }

        public int[] GenerateTriangles() {
            int[] triangles = new int[xSize * ySize * 6];

            for (int z = 0, i = 0; z < ySize; z++) {
                for (int x = 0; x < xSize; x++) {
                    triangles[i]     = (int)((z * (xSize + 1)) + x);
                    triangles[i + 1] = (int)(((z + 1) * (xSize + 1)) + x);
                    triangles[i + 2] = (int)((z * (xSize + 1)) + x + 1);
                    triangles[i + 3] = (int)(((z + 1) * (xSize + 1)) + x);
                    triangles[i + 4] = (int)(((z + 1) * (xSize + 1)) + x + 1);
                    triangles[i + 5] = (int)((z * (xSize + 1)) + x + 1);
                    
                    i += 6;
                }
            }

            return triangles;
        }

        public Vector2[] GenerateUVs() {
            Vector2[] uvs = new Vector2[(xSize + 1) * (ySize + 1)];

            for (int z = 0, i = 0; z <= ySize; z++) {
                for (int x = 0; x <= xSize; x++) {
                    uvs[i] = new Vector2((float)x / xSize, (float)z / ySize);
                    i++;
                }
            }

            return uvs;
        }

        public Mesh UpdatedMesh() {
            // Null check the mesh
            if (!mesh) mesh = new Mesh();

            mesh.Clear();
            mesh.vertices = GenerateVertices();
            mesh.triangles = GenerateTriangles();
            mesh.uv = GenerateUVs();
            mesh.RecalculateNormals();
            return mesh;
        }

        public MeshGenerator(Texture2D noiseTexture, float heightMultiplier) {
            if (heightMultiplier <= 0) {
                Debug.LogError("Invalid height multiplier value");
                return;
            }

            xSize = noiseTexture.width;
            ySize = noiseTexture.height;
            this.noiseTexture = noiseTexture;
            this.heightMultiplier = heightMultiplier;
        }
    }
}
