using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class CreateTerrain
{
    //Globals
    int width; // X Axis size
    int height; // Z Axis size
    int depth; // Y Axis size
    float scale;
    int worldSeed; 
    int octaves; //Amount of heightmaps to stack
    float persistance; //Controls how much an octave effects the final heightmap 
    float lacunarity; //Controls details of octaves in the heightmap
    float offsetX;
    float offsetY;

    //Generate a terrain method
    public TerrainData generateTerrain(TerrainSettings terrainsettings)
    {
        try
        {
            //Get variables from terrainsettings class and save to local instance
            width = terrainsettings.Width;
            height = terrainsettings.Height;
            depth = terrainsettings.Depth;
            scale = terrainsettings.Scale;
            worldSeed = terrainsettings.WorldSeed;
            octaves = terrainsettings.Octaves;
            persistance = terrainsettings.Persistance;
            lacunarity = terrainsettings.Lacunarity;
            offsetX = terrainsettings.OffsetX;
            offsetY = terrainsettings.OffsetY;

            //Variables for loading bar
            float completeValue = 30f;
            float progress = 0f;

            progress = progress + 10;

            //Check if size is valid (power of 2 - 1, if not make it)
            if (isValueToThePowerOfTwo(width - 1) == false)
            {
                int y = width;
                while (isValueToThePowerOfTwo(y) == false)
                {
                    y++;
                }
                width = y + 1;
                height = y + 1;
            }


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

            terrainData.heightmapResolution = width;
            terrainData = generateHeightMap(terrainData);

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
    TerrainData generateHeightMap(TerrainData terrainData)
    {
   
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        //Create the noise map using perlin noise
        float[,] noiseMap = new float[width, height];

        //World Seed generator
        System.Random prng = new System.Random(worldSeed);

        //Offset of NoiseMap
        Vector2 offset = new Vector2(offsetX, offsetY);

        //Create min and max value for noiseheight through use of floats min/maxvalue function
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;



        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {

                float amplitude = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {

                    float sampleX = (x - width) / scale  + offset.x;
                    float sampleY = (y - height) / scale  + offset.y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeight += perlinValue * amplitude;

                }

                //Failsafe to stop min and max noise
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                //Set Noise height at this map location
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
        terrainData.SetHeights(0, 0, noiseMap);

        return terrainData;
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

        //Get amount of textures from folder and add all to an array of splat protos
        int texturesAvaliabale = dirCount(new DirectoryInfo(Application.dataPath + "/Drews-WorldGen/Resources/Textures"));
        Debug.Log(texturesAvaliabale);

        SplatPrototype[] texturesArray = new SplatPrototype[texturesAvaliabale];

        for (int x = 0; x < texturesAvaliabale; x++)
        {
            SplatPrototype newSplat = new SplatPrototype();
            int textureFileName = x + 1;
            newSplat.texture = (Texture2D)Resources.Load("Textures/" + textureFileName);
            newSplat.tileSize = new Vector2(4f, 4f);
            texturesArray[x] = newSplat;
        }

        //Apply Textures
        terrainData.splatPrototypes = texturesArray;

        terrainData.RefreshPrototypes();
        terrainData.SetAlphamaps(0, 0, makeSplatmap(terrainData));

        return terrainData;
    }


    float[,,] makeSplatmap(TerrainData terrainData)
    {

        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
        Debug.Log(terrainData.alphamapHeight);
        Debug.Log(terrainData.alphamapWidth);

        //Work out the texture split 
        int textureSize = terrainData.splatPrototypes.Length;
        int textureProportion = depth / textureSize;

        int[] minTextureValue = new int[textureSize];
        int[] maxTextureValue = new int[textureSize];

        for (int x = 0; textureSize > x; x++)
        {
            minTextureValue[x] = textureProportion * (x);
            maxTextureValue[x] = minTextureValue[x] + depth;
        }

        Debug.Log("Height :" + terrainData.alphamapHeight);
        Debug.Log("Width :" + terrainData.alphamapWidth);

        //Loop for each point
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                //Check each texture
                for (int z = 0; textureSize > z; z++)
                {
                    //Compare size to proportions
                    if (terrainData.GetHeight(x, y) < (depth / (z + 1)))
                    {
                        for (int a = 0; textureSize > a; a++)
                        {
                            splatmapData[y, x, a] = 0f;
                        }
                        splatmapData[y, x, z] = 1f;
                    }
                }
                //terrainData.GetHeight(x, y) >= minTextureValue[z] && terrainData.GetHeight(x, y) <= maxTextureValue[z])

            }
        }

        return splatmapData;
    }

    //Add Grass to terraindata
    TerrainData grassForTerrain(TerrainData terrainData)
    {
        int grassDensity = 5;
        int patchDetail = 2;

        terrainData.SetDetailResolution(grassDensity, patchDetail);

        int[,] newMap = new int[grassDensity, grassDensity];

        DetailPrototype[] grass = new DetailPrototype[1];

        grass[0] = new DetailPrototype();

        grass[0].prototypeTexture = Resources.Load("Grass/" + "0") as Texture2D;

        terrainData.detailPrototypes = grass;

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

        terrainData.SetDetailResolution(10, 100);
        terrainData.SetDetailLayer(0, 0, 0, newMap);

        return terrainData;
    }

  
    public static void generateTrees(Terrain terrain)
    {

        TerrainData terrainData = terrain.terrainData;


        int numOfTrees = 4;

        TreePrototype[] treeProtoTypes = new TreePrototype[4];
        GameObject[] trees = new GameObject[numOfTrees];

        trees[0] = Resources.Load("Trees/" + "Tree1") as GameObject;
        trees[1] = Resources.Load("Trees/" + "Tree2") as GameObject;
        trees[2] = Resources.Load("Trees/" + "Tree3") as GameObject;
        trees[3] = Resources.Load("Trees/" + "Tree4") as GameObject;

        for (int i = 0; i < numOfTrees; i++)
        {
            treeProtoTypes[i] = new TreePrototype();
            treeProtoTypes[i].prefab = trees[i];
        }

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

    //Check if value is to the power of two
    bool isValueToThePowerOfTwo(int x)
    {
        return (x & (x - 1)) == 0;
    }

    //Count files in a directory excluding meta files
    int dirCount(DirectoryInfo d)
    {
        int i = 0;

        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("meta")) { }

            else { i++; }
        }
        return i;
    }
}
