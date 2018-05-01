using System;
using UnityEngine;

/**
 * The following class is used for random generation of the terrain, it also allows for user defined settings to be passed to it for less random terrains
 **/
public class RandomGeneration
{

    //Class Wide Variables
    bool waterEnabled;
    TerrainSettings terrainSettings = new TerrainSettings();

    //Used to create a completly random terrain within the applications set height and width parameters
    public void proceduralGeneration(int mapSize)
    {

        //Set settings values 
        terrainSettings.Width = mapSize;
        terrainSettings.Height = mapSize;
        terrainSettings.Depth = randomInt(1, 50);
        terrainSettings.Scale = randomFloat(0.0001f, 30.00f);
        terrainSettings.WorldSeed = randomInt(1, 10000);
        terrainSettings.Octaves = randomInt(1, 10);
        terrainSettings.Persistance = randomFloat(0.01f, 1.00f);
        terrainSettings.Lacunarity = randomFloat(0.01f, 20.00f);
        terrainSettings.OffsetX = randomFloat(0.00f, 500.00f);
        terrainSettings.OffsetY = randomFloat(0.00f, 500.00f);
        waterEnabled = false;

        //Generate terrain
        generateTerrain();
    }


    //Used to create a completly random terrain within the applications set height and width parameters
    public void proceduralGenerationCustomSettings(int mapSize, int Depth, float Scale, int worldSeed, int Octaves, float Persistance, float Lacunarity, bool WaterEnabled)
    {

        //Set variables
        terrainSettings.Width = mapSize;
        terrainSettings.Height = mapSize;
        terrainSettings.OffsetX = randomFloat(0.00f, 500.00f);
        terrainSettings.OffsetY = randomFloat(0.00f, 500.00f);
        waterEnabled = WaterEnabled;

        //Values will be randomised if value passed from settings is equal to 0
        if (Depth == 0) { terrainSettings.Depth = randomInt(20, 100); }
        else { terrainSettings.Depth = Depth; }
        if (Scale == 0) { terrainSettings.Scale = randomFloat(0.0001f, 50.00f); }
        else { terrainSettings.Scale = Scale; }
        if (worldSeed == 0) { terrainSettings.WorldSeed = randomInt(1, 10000); }
        else { terrainSettings.WorldSeed = worldSeed; }
        if (Octaves == 0) { terrainSettings.Octaves = randomInt(1, 5); }
        else { terrainSettings.Octaves = Octaves; }
        if (Persistance == 0) { terrainSettings.Persistance = randomFloat(0.01f, 1.00f); }
        else { terrainSettings.Persistance = Persistance; }
        if (Lacunarity == 0) { terrainSettings.Lacunarity = randomFloat(0.01f, 20.00f); }
        else { terrainSettings.Lacunarity = Lacunarity; }

        //Generate terrain
        generateTerrain();
    }

    //Generate terrain and any settings set by it
    private void generateTerrain()
    {

        //Class for terrain settings object
        CreateTerrain createTerrain = new CreateTerrain();

        //Create the terrain in world
        GameObject generatedTerrain = Terrain.CreateTerrainGameObject(createTerrain.generateTerrain(terrainSettings));


        //Check if water has enabled, if so create it in scene
        if (waterEnabled == true)
        {
            try
            {
                //Create a game object callled water from resources
                GameObject water = GameObject.Instantiate(Resources.Load("Water") as GameObject);
                water.transform.localScale = new Vector3(terrainSettings.Height * 2, 1, terrainSettings.Width * 2);
                water.transform.localPosition = new Vector3(0, 30, 0);
            }
            catch (Exception exception)
            {
                Debug.Log("Water needs to be added as a game object within resource folder before being generated");
                throw new ApplicationException("Terrain Generator has failed with the folloing exception : \n : ", exception);
            }
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
