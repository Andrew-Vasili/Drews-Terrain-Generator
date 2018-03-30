using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSettings
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }
    public float Scale { get; set; }
    public int Seed { get; set; }
    public int Octaves { get; set; }
    public float Persistance { get; set; }
    public float Lacunarity { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }

    //Create a terrain object
   public void setupTerrain(int width, int height, int depth, float scale, int seed, int octaves, float persistance, float lacunarity, float offsetX, float offsetY)
    {

        Width = width;
        Height = height;
        Depth = depth;
        Scale = scale;
        Seed = seed;
        Octaves = octaves;
        Persistance = persistance;
        Lacunarity = lacunarity;
        OffsetX = offsetX;
        OffsetY = offsetY;

    }
}
