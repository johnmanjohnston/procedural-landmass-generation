# procedural-landmass-generation
Procedurally generates landmass, using Unity, and C#

### About
This was written in C#, using Unity. The project procedurally generates landmass/terrain like mountains, and hills. 
It uses perlin noise, and the mesh for the terrain is generated according to a texture, generated as per the noise.

### Demo Screenshots
![image](https://user-images.githubusercontent.com/97091148/181032734-644ab7ac-1a99-4f31-a620-458d5c3331b6.png)
![image](https://user-images.githubusercontent.com/97091148/181033288-e67e5859-2e98-4341-b544-61c68cb0bb88.png)

### Explanation
The terrain shape is decided using perlin noise, and is made in the `NoiseGenerator` class, in the `NoisGenerator.cs` file.
It has a Generate() function, which takes in the following arguments:
```
uint width, 
uint height, 
float xOffset, 
float yOffset, 
float scale,

uint octaves = 8
```

The function validates the arguments, and then generates the noise as a two-dimensional float array.
The file also has a function to get the noise as a texture, which takes the noise data as an argument, `NoiseTexture(float[,] noise)`.

The `MeshGenerator` class has functions to generate the mesh, according to the noise texture. It looks at the noise texture and generates the mesh accordingly.
The class has functions for generating the vertices, and triangles, of the mesh, and the class utilizes them to generate the mesh.

The `TerrainGenerator` class has customizable parameters for generating the noise, which generates the terrain, which can be tweaked inside of the Unity Inspector. 
The file also updates the noise texture on its own, by incrementing the `xOffset` and `yOffset` variables, and then reaquires the noise, and updates the terrain mesh.
