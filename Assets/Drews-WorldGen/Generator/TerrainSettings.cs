using UnityEngine;

/**
 * The following class holds the values for the terrains to be generated
 **/
public class TerrainSettings
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }
    public float Scale { get; set; }
    public int WorldSeed { get; set; }
    public int Octaves { get; set; }
    public float Persistance { get; set; }
    public float Lacunarity { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }
    public AnimationCurve heightCurve { get; set; }

    //Create a terrain object
    public void setupTerrain(int width, int height, int depth, float scale, int worldSeed, int octaves, float persistance, float lacunarity, float offsetX, float offsetY)
    {

        Width = width;
        Height = height;
        Depth = depth;
        Scale = scale;
        WorldSeed = worldSeed;
        Octaves = octaves;
        Persistance = persistance;
        Lacunarity = lacunarity;
        OffsetX = offsetX;
        OffsetY = offsetY;

    }
}
