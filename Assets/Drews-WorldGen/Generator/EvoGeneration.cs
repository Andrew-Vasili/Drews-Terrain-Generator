using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * The following class is used for the random generation of the terrain through the use of genetic algorithms 
 **/
public class EvoGeneration : EditorWindow
{

    //Create a list object of a new population that contains new indivduals defined by terrain settings
    public List<TerrainSettings> createPopulation(int populationToCreate, int mapSize)

    {
        //Setup starting varaibles
        List<TerrainSettings> population = new List<TerrainSettings>(); //Current Population

        //Create a new indivdual and add it to the population for every indivdual needed as specified by the 'populationToCreate' variable
        for (int x = 0; x < populationToCreate;)
        {

            //Setup the terrain settings object
            TerrainSettings terrainSettings = new TerrainSettings();

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

            //Create terrain settings object
            population.Add(terrainSettings);
            x++;
        }
        return population;
    }

    //Create a new generation through the use of several genetic operators and individuals selected from the last generation
    public List<TerrainSettings> newGeneration(List<TerrainSettings> parents, int populationSize, int worldSize)
    {
        //Population object of new generation
        List<TerrainSettings> newPopulation = new List<TerrainSettings>();

        //Get amount of parents selected from the indiviudals of the last generation
        int amountOfParents = parents.Count;

        //Get 75% of population to create value which in turn is used to figure out how many children need to be created and how many to be added for diversity
        int childrenToCreateFromParents = (populationSize * 3) / 4;

        //Get remaining figure for random generation (Diversity individuals)
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

            //Crossover chance of 85%, if above then parent(A) fully taken to next generation without crossover using a multipoint crossover technique 
            if (randomInt(1, 100) > 85)
            {
                newPopulation.Add(parents[parent1]);
            }
            else
            {
                //Mutation rate set at 1%

                //Create settings object to add to list
                TerrainSettings terrainSettings = new TerrainSettings();

                //Height and width size is defined for all terrains
                terrainSettings.Width = worldSize;
                terrainSettings.Height = worldSize;

                //Depth
                if (randomInt(1, 100) == 100)
                {
                    terrainSettings.Depth = randomInt(1, 50);
                }
                else
                {
                    if (randomInt(1, 100) < 50)
                    {
                        terrainSettings.Depth = parents[parent1].Depth;
                    }
                    else
                    {
                        terrainSettings.Depth = parents[parent2].Depth;
                    }
                }

                //Scale
                if (randomInt(1, 100) == 100)
                {
                    terrainSettings.Scale = randomFloat(0.0001f, 30.00f);
                }
                else
                {
                    if (randomInt(1, 100) < 50)
                    {
                        terrainSettings.Scale = parents[parent1].Scale;
                    }
                    else
                    {
                        terrainSettings.Scale = parents[parent2].Scale;
                    }
                }

                //worldSeed
                if (randomInt(1, 100) == 100)
                {
                    terrainSettings.WorldSeed = randomInt(1, 10000);
                }
                else
                {
                    if (randomInt(1, 100) < 50)
                    {
                        terrainSettings.WorldSeed = parents[parent1].WorldSeed;
                    }
                    else
                    {
                        terrainSettings.WorldSeed = parents[parent2].WorldSeed;
                    }
                }

                //Octaves
                if (randomInt(1, 100) == 100)
                {
                    terrainSettings.Octaves = randomInt(1, 10);
                }
                else
                {
                    if (randomInt(1, 100) < 50)
                    {
                        terrainSettings.Octaves = parents[parent1].Octaves;
                    }
                    else
                    {
                        terrainSettings.Octaves = parents[parent2].Octaves;
                    }
                }

                //Persistance
                if (randomInt(1, 100) == 100)
                {
                    terrainSettings.Persistance = randomFloat(0.01f, 1.00f);
                }
                else
                {
                    if (randomInt(1, 100) < 50)
                    {
                        terrainSettings.Persistance = parents[parent1].Persistance;
                    }
                    else
                    {
                        terrainSettings.Persistance = parents[parent2].Persistance;
                    }
                }

                //Lacunarity
                if (randomInt(1, 100) == 100)
                {
                    terrainSettings.Lacunarity = randomFloat(0.01f, 20.00f);
                }
                else
                {
                    if (randomInt(1, 100) < 50)
                    {
                        terrainSettings.Lacunarity = parents[parent1].Lacunarity;
                    }
                    else
                    {
                        terrainSettings.Lacunarity = parents[parent2].Lacunarity;
                    }
                }
                //OffsetX

                if (randomInt(1, 100) == 100)
                {
                    terrainSettings.OffsetX = randomFloat(0.00f, 500.00f);

                }
                else
                {
                    if (randomInt(1, 100) < 50)
                    {
                        terrainSettings.OffsetX = parents[parent1].OffsetX;
                    }
                    else
                    {
                        terrainSettings.OffsetX = parents[parent2].OffsetX;
                    }
                }

                //OffsetY
                if (randomInt(1, 100) == 100)
                {
                    terrainSettings.OffsetY = randomFloat(0.00f, 500.00f);

                }
                else
                {
                    if (randomInt(1, 100) < 50)
                    {
                        terrainSettings.OffsetY = parents[parent1].OffsetY;
                    }
                    else
                    {
                        terrainSettings.OffsetY = parents[parent2].OffsetY;
                    }
                }

                //Add Child to next generation pool
                newPopulation.Add(terrainSettings);

            }
            x++;
        }

        //Fills in population with new individuals for diversity 


        //Creates new individuals and adds to population
        List<TerrainSettings> newIndividuals = createPopulation(childrenToCreateFromRandom, worldSize);
        newPopulation.AddRange(newIndividuals);

 
        //Return new population
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
