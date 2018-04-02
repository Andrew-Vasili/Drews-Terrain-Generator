using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EvoGeneration : EditorWindow
{

    
    //Begin the process 
    public List<TerrainSettings> startIntialPopulation()
    {
        //Setup starting varaibles
        List<TerrainSettings> population = new List<TerrainSettings>(); //Current Population
        

        for (int x = 0; x < 10;)
        {
            Debug.Log("Creating population : " + (x + 1));

            TerrainSettings terrainSettings = new TerrainSettings();
            //Create random values 
            int width = 1000;
            int height = 1000;
            int depth = randomInt(20, 100);
            float scale = randomFloat(0.0001f, 50.00f);
            int seed = randomInt(1, 10000);
            int octaves = randomInt(1, 5);
            float persistance = randomFloat(0.01f, 1.00f);
            float lacunarity = randomFloat(0.01f, 20.00f);
            float offsetX = randomFloat(0.00f, 500.00f);
            float offsetY = randomFloat(0.00f, 500.00f);

            //Create terrain settings object
            terrainSettings.setupTerrain(width, height, depth, scale, seed, octaves, persistance, lacunarity, offsetX, offsetY);

            population.Add(terrainSettings);
            x++;
        }
        return population;
    }


    public List<TerrainSettings> newGeneration(List<TerrainSettings> parents)
    {
        List<TerrainSettings> newPopulation = new List<TerrainSettings>(); //Current Population

        int amountOfParents = parents.Count;




        return newPopulation;

    }

    // Get random int value between X and Y
    private int randomInt(int min, int max)
    {
        return Random.Range(min, max);

    }

    // Get random float value between X and Y
    private float randomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }


}
