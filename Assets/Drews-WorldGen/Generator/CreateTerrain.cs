using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateTerrain : MonoBehaviour
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
    float Length = 50f;
    float Height = 50f;

    //Generate a terrain method
    public TerrainData generateTerrain(TerrainSettings terrainsettings)
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
        terrainData.heightmapResolution = Resolution;
        terrainData = GenerateHeightMap(terrainData);

        //Set Textures
        terrainData =  textureTerrain(terrainData);

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
        //Set noise map array
        float[,] noiseMap = new float[width, height];

        //Seed generator to get same terrains in future generations
        System.Random seedGen = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = octaves; i < octaves; i++)
        {
            float x = seedGen.Next(-100000, 100000) + offsetX;
            float y = seedGen.Next(-100000, 100000) + offsetY;
            octaveOffsets[i] = new Vector2(x, y);
        }

        //Tracker for heightmap values
        float maxNoise = float.MinValue;
        float minNoise = float.MaxValue;

        //Loop for each width pixel
        for (int x = 0; x < width; x++)
        {
            //Loop for each height pixel
            for (int y = 0; y < height; y++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //Loop for each octave set
                for (int z = 0; z < octaves; z++)
                {
                    float noiseX = x / scale * frequency + octaveOffsets[z].x;
                    float noiseY = y / scale * frequency + octaveOffsets[z].y;

                    float perlinValue = Mathf.PerlinNoise(noiseX, noiseY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    frequency *= lacunarity;
                }

                //Checking values don't go over or under
                if (noiseHeight > maxNoise)
                {
                    maxNoise = noiseHeight;
                }
                else if (noiseHeight < minNoise)
                {
                    minNoise = noiseHeight;
                }

                //Convert Values
                noiseMap[x, y] = noiseHeight;
            }
        }

        //Loop for each width pixel
        for (int x = 0; x < width; x++)
        {
            //Loop for each height pixel
            for (int y = 0; y < height; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x, y]);
            }
        }

        //Return noisemap
        return noiseMap;
    }


    public float[,] MakeHeightmap()
{
    float[,] Heightmap = new float[Resolution, Resolution];

    for (int x = 0; x < Resolution; x++)
        for (int z = 0; z < Resolution; z++)
        {
            Heightmap[x, z] = GetNormalizedHeight((float)x, (float)z);
        }

    return Heightmap;
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


public float GetNormalizedHeight(float x, float z)
{
    return Mathf.Clamp(Mathf.PerlinNoise(x * 0.05f, z * 0.05f), 0f, 0.4f) * 0.95f + Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 0.05f;
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
    int valueToReturn = Random.Range(min, max);
    return valueToReturn;
}
}