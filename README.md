# procedural-landmass-generation
Procedurally generates landmass, using Unity, and C#

### About
This was written in C#, using Unity. The project procedurally generates landmass/terrain like mountains, and hills. 
It uses perlin noise, and the mesh for the terrain is generated according to a texture, generated as per the noise.

It also includes a basic camera controller which lets you navigate, and move around the simulation, and the terrain.
The terrain also has optional color to it which can be toggled on and off. 
The noise texture used for the terrain is previewed on the top right of the screen, and the color map used for the terrain is shown on the top left of the screen.

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
