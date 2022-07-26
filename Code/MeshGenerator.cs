using UnityEngine;

namespace TerrainGeneration.MeshGeneration {
    public class MeshGenerator
    {
        private uint xSize;
        private uint ySize;
        private Texture2D noiseTexture;
        private float heightMultiplier;

        private Mesh mesh;

        public Vector3[] GenerateVertices() {
            Vector3[] vertices = new Vector3[(xSize + 1) * (ySize + 1)];

            // this took so long to get right i never want to deal with crap like this ever again
            for (int z = 0, i = 0; z <= ySize; z++) {
                for (int x = 0; x <= xSize; x++) {
                    float y = noiseTexture.GetPixel(x, z).grayscale * heightMultiplier;
                    vertices[i] = new Vector3(x, y * 2f, z);
                    i++;
                }
            }

            return vertices;
        }

        public int[] GenerateTriangles() {
            int[] triangles = new int[xSize * ySize * 6];

            // thank you github copilot you saved me from losing my sanity here
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

        public Mesh UpdatedMesh() {
            // Null check the mesh
            if (!mesh) mesh = new Mesh();

            mesh.Clear();
            mesh.vertices = GenerateVertices();
            mesh.triangles = GenerateTriangles();
            mesh.RecalculateNormals();
            return mesh;
        }

        public MeshGenerator(uint xSize, uint ySize, Texture2D noiseTexture, float heightMultiplier) {
            this.xSize = xSize;
            this.ySize = ySize;
            this.noiseTexture = noiseTexture;
            this.heightMultiplier = heightMultiplier;
        }
    }
}
