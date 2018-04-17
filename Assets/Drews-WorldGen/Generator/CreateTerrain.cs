using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class CreateTerrain
{
    //Globals
    int width; // X Axis size
    int height; // Z Axis size
    int depth; // Y Axis size
    float scale;
    int seed; //World ID
    int octaves; //Levels of noise in perlin noise heightmap
    float persistance; //Controls how much an octave effects the final heightmap 
    float lacunarity; //Controls details of octaves in the heightmap
    float offsetX;
    float offsetY;

    int Resolution = 129;

    //Generate a terrain method
    public TerrainData generateTerrain(TerrainSettings terrainsettings)
    {
        try
        {
            //Get variables from createterrain class and save to local instance
            width = terrainsettings.Width;
            height = terrainsettings.Height;
            depth = terrainsettings.Depth;
            scale = terrainsettings.Scale;
            seed = terrainsettings.Seed;
            octaves = terrainsettings.Octaves;
            persistance = terrainsettings.Persistance;
            lacunarity = terrainsettings.Lacunarity;
            offsetX = terrainsettings.OffsetX;
            offsetY = terrainsettings.OffsetY;

            //Variables for loading bar
            float completeValue = 30f;
            float progress = 0f;

            EditorUtility.DisplayProgressBar("Generating...", "Generating terrain, please be patient!", progress / completeValue);

            //Intialise terrain object settings
            TerrainData terrainData = new TerrainData();

            progress = progress + 10;
            EditorUtility.DisplayProgressBar("Generating...", "Randomizing those values, please be patient!", progress / completeValue);

            //50/50 chance of Offset having mathing values
            int x = randomValue(1, 10);
            if (x <= 5)
            {
                offsetX = randomValue(1, 200);
                offsetY = randomValue(1, 200);
            }
            else
            {
                int y = randomValue(1, 200);
                offsetX = y;
                offsetY = y;
            }
            progress = progress + 10;
            EditorUtility.DisplayProgressBar("Generating...", "Creating base settings for your terrain, please be patient!", progress / completeValue);

            //Set base settings
            terrainData.alphamapResolution = Resolution;
            terrainData.heightmapResolution = randomEven(200, 500) + 1;
            terrainData = GenerateHeightMap(terrainData);

            //Set Textures
            terrainData = textureTerrain(terrainData);

            //Add grass to world
            // terrainData = grassForTerrain(terrainData);

            progress = progress + 10;
            EditorUtility.DisplayProgressBar("Generating...", "Creating empty terrain model, please be patient!", progress / completeValue);

            //Generate base terrain
            progress = progress + 10;

            EditorUtility.DisplayProgressBar("Generating...", "Adding finshing touches, please be patient!", progress / completeValue);

            EditorUtility.ClearProgressBar();

            return terrainData;
        }
        catch (Exception exception)
        {
            EditorUtility.ClearProgressBar();
            throw new ApplicationException("Terrain Generator has failed with the folloing exception : \n : ", exception);
        }
    }

    //Create the heightmap using perlin noise
    TerrainData GenerateHeightMap(TerrainData terrainData)
    {

        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    //Create the noise map using perlin noise
    float[,] GenerateHeights()
    {
        float[,] noiseMap = new float[width, height];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }


        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;


        float halfWidth = width / 2f;
        float halfHeight = height / 2f;


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {

                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;

                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }


    TerrainData randomTextures(TerrainData terrainData)
    {
        DirectoryInfo dir = new DirectoryInfo("Textures");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        { Debug.Log("f"); }
        return terrainData;
    }
    //Add Texture's to terraindata
    TerrainData textureTerrain(TerrainData terrainData)
    {
        //Set textures
        SplatPrototype Sand = new SplatPrototype();
        SplatPrototype Rock = new SplatPrototype();
        Sand.texture = (Texture2D)Resources.Load("Textures/Forest");
        Sand.tileSize = new Vector2(4f, 4f);
        Rock.texture = (Texture2D)Resources.Load("Textures/Rock");
        Rock.tileSize = new Vector2(4f, 4f);

        //Apply Textures
        terrainData.splatPrototypes = new SplatPrototype[] { Sand, Rock };
        terrainData.RefreshPrototypes();
        terrainData.SetAlphamaps(0, 0, MakeSplatmap(terrainData));

        return terrainData;
    }

    public float[,,] MakeSplatmap(TerrainData TerrainData)
    {
        float[,,] Splatmap = new float[Resolution, Resolution, 2];

        for (int x = 0; x < Resolution; x++)
            for (int z = 0; z < Resolution; z++)
            {
                float NormalizedX = (float)x / ((float)Resolution - 1f);
                float NormalizedZ = (float)z / ((float)Resolution - 1f);

                float Steepness = TerrainData.GetSteepness(NormalizedX, NormalizedZ) / 90f;

                Splatmap[z, x, 0] = 1f - Steepness;
                Splatmap[z, x, 1] = Steepness;
            }

        return Splatmap;
    }
    //Add Grass to terraindata
    TerrainData grassForTerrain(TerrainData terrainData)
    {
        int grassDensity = 5;
        int patchDetail = 2;

        terrainData.SetDetailResolution(grassDensity, patchDetail);

        int[,] newMap = new int[grassDensity, grassDensity];

        for (int i = 0; i < grassDensity; i++)
        {
            for (int j = 0; j < grassDensity; j++)
            {
                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = terrainData.GetHeight(i, j);
                if (height < 10.0f)
                {
                    newMap[i, j] = 6;
                }
                else
                {
                    newMap[i, j] = 0;
                }
            }
        }
        terrainData.SetDetailLayer(0, 0, 0, newMap);

        return terrainData;
    }
    // Get random value between X and Y
    private int randomValue(int min, int max)
    {
        int valueToReturn = UnityEngine.Random.Range(min, max);
        return valueToReturn;
    }

    // Generate even number between X and Y 
    private int randomEven(int min, int max)
    {
        int valueToReturn = 2 * UnityEngine.Random.Range(min, max);
        return valueToReturn;
    }
}