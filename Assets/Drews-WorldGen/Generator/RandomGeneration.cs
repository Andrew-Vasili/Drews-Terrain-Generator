using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGeneration
{

    //Used to create a completly random terrain within the applications set height and width parameters
    public void proceduralGeneration(int mapSize)
    {
        //Class for terrain settings object
        CreateTerrain createTerrain = new CreateTerrain();

        //Create random values 
        int width = mapSize;
        int height = mapSize;
        int depth = randomInt(20, 100);
        float scale = randomFloat(0.0001f, 50.00f);
        int seed = randomInt(1, 10000);
        int octaves = randomInt(1, 5);
        float persistance = randomFloat(0.01f, 1.00f);
        float lacunarity = randomFloat(0.01f, 20.00f);
        float offsetX = randomFloat(0.00f, 500.00f);
        float offsetY = randomFloat(0.00f, 500.00f);

        //Create terrain settings object
        TerrainSettings terrainSettings = new TerrainSettings();
        terrainSettings.setupTerrain(width, height, depth, scale, seed, octaves, persistance, lacunarity, offsetX, offsetY);

        //Create the terrain in world
        GameObject generatedTerrain = Terrain.CreateTerrainGameObject(createTerrain.generateTerrain(terrainSettings));

        GameObject water = GameObject.Instantiate(Resources.Load("Water") as GameObject);
        water.transform.localScale = new Vector3(height * 2, 1, width * 2);
        water.transform.localPosition = new Vector3(0, 30, 0);
    }


    //Used to create a completly random terrain within the applications set height and width parameters
    public void proceduralGenerationCustomSettings(int mapSize, int Depth, float Scale, int Seed, int Octaves, float Persistance, float Lacunarity, float OffsetX, float OffsetY, bool waterEnabled)
    {
        //Class for terrain settings object
        CreateTerrain createTerrain = new CreateTerrain();

        Debug.Log(waterEnabled);

        //Set variables to create
        int width = mapSize;
        int height = mapSize;
        int depth;
        float scale;
        int seed;
        int octaves;
        float persistance;
        float lacunarity;
        float offsetX;
        float offsetY;

        //Values will be randomised if value passed from settings is equal to 0
        if (Depth == 0) { depth = randomInt(20, 100); }
        else { depth = Depth; }

        if (Scale == 0) { scale = randomFloat(0.0001f, 50.00f); }
        else { scale = Scale; }

        if (Seed == 0) { seed = randomInt(1, 10000); }
        else { seed = Seed; }

        if (Octaves == 0) { octaves = randomInt(1, 5); }
        else { octaves = Octaves; }

        if (Persistance == 0) { persistance = randomFloat(0.01f, 1.00f); }
        else { persistance = Persistance; }

        if (Lacunarity == 0) { lacunarity = randomFloat(0.01f, 20.00f); }
        else { lacunarity = Lacunarity; }

        if (OffsetX == 0) { offsetX = randomFloat(0.00f, 500.00f); }
        else { offsetX = OffsetX; }

        if (OffsetY == 0) { offsetY = randomFloat(0.00f, 500.00f); }
        else { offsetY = OffsetY; };

        //Create terrain settings object
        TerrainSettings terrainSettings = new TerrainSettings();
        terrainSettings.setupTerrain(width, height, depth, scale, seed, octaves, persistance, lacunarity, offsetX, offsetY);

        //Create the terrain in world
        GameObject generatedTerrain = Terrain.CreateTerrainGameObject(createTerrain.generateTerrain(terrainSettings));

        if (waterEnabled == true)
        {
            GameObject water = GameObject.Instantiate(Resources.Load("Water") as GameObject);
            water.transform.localScale = new Vector3(height * 2, 1, width * 2);
            water.transform.localPosition = new Vector3(0, 30, 0);
        }
    }


    // Get random int value between X and Y
    private int randomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);

    }

    // Get random float value between X and Y
    private float randomFloat(float min, float max)
    {

        return UnityEngine.Random.Range(min, max);

    }

}
