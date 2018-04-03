using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EvoGeneration : EditorWindow
{


    //Begin the create a new population of indivduals  
    public List<TerrainSettings> createPopulation(int populationToCreate)
    {
        //Setup starting varaibles
        List<TerrainSettings> population = new List<TerrainSettings>(); //Current Population


        for (int x = 0; x < populationToCreate;)
        {
            Debug.Log("Creating random indivudal #" + (x + 1));

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


    public List<TerrainSettings> newGeneration(List<TerrainSettings> parents, int populationSize)
    {
        List<TerrainSettings> newPopulation = new List<TerrainSettings>(); //Population of next generation

        int amountOfParents = parents.Count;

        //Get 75% to figure out how many children to create
        int childrenToCreateFromParents = (populationSize * 3) / 4;

        //Get remaining figure for random generation
        int childrenToCreateFromRandom = populationSize - childrenToCreateFromParents;

        //Crossover operator 
        for (int x = 0; x < childrenToCreateFromParents;)
        {

            //Get parents via random allocation
            int parent1 = randomInt(0, parents.Count - 1);
            int parent2 = randomInt(0, parents.Count - 1);

            //Prevent both parents being the same object
            while (parent2 == parent1)
            {
                parent2 = randomInt(0, parents.Count - 1);
            }

            //Crossover chance of 85% if above then parent(A) fully taken to next generation without crossover using a multipoint crossover technique 
            if (randomInt(1, 100) > 85)
            {
                newPopulation.Add(parents[parent1]);
            }
            else
            {

                TerrainSettings terrainSettings = new TerrainSettings();

                //Width
                if (randomInt(1, 2) == 1){
                    terrainSettings.Width = parents[parent1].Width;
                }
                else
                {
                    terrainSettings.Width = parents[parent2].Width;
                }

                //Height
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.Height = parents[parent1].Height;
                }
                else
                {
                    terrainSettings.Height = parents[parent2].Height;
                }

                //Depth
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.Depth = parents[parent1].Depth;
                }
                else
                {
                    terrainSettings.Depth = parents[parent2].Depth;
                }

                //Scale
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.Scale = parents[parent1].Scale;
                }
                else
                {
                    terrainSettings.Scale = parents[parent2].Scale;
                }

                //Seed
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.Seed = parents[parent1].Seed;
                }
                else
                {
                    terrainSettings.Seed = parents[parent2].Seed;
                }

                //Octaves
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.Octaves = parents[parent1].Octaves;
                }
                else
                {
                    terrainSettings.Octaves = parents[parent2].Octaves;
                }

                //Persistance
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.Persistance = parents[parent1].Persistance;
                }
                else
                {
                    terrainSettings.Persistance = parents[parent2].Persistance;
                }

                //Lacunarity
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.Lacunarity = parents[parent1].Lacunarity;
                }
                else
                {
                    terrainSettings.Lacunarity = parents[parent2].Lacunarity;
                }

                //OffsetX
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.OffsetX = parents[parent1].OffsetX;
                }
                else
                {
                    terrainSettings.OffsetX = parents[parent2].OffsetX;
                }

                //OffsetY
                if (randomInt(1, 2) == 1)
                {
                    terrainSettings.OffsetY = parents[parent1].OffsetY;
                }
                else
                {
                    terrainSettings.OffsetY = parents[parent2].OffsetY;
                }
           
                //Add Child to next generation pool
                newPopulation.Add(terrainSettings);
        

            }
            x++;
        }

        //Fill in population with new individuals for diversity
        if (newPopulation.Count < populationSize)
        {
            int x = 10 - newPopulation.Count;

            List<TerrainSettings> newIndividuals = createPopulation(x); //New individuals to add to population

        }


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
