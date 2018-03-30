using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGeneration : MonoBehaviour
{

    CreateTerrain createTerrain = new CreateTerrain();

    int width;
    int height;
    int depth;
    float scale;
    int seed;
    int octaves;
    float persistance;
    float lacunarity;
    float offsetX;
    float offsetY;

    //Used to create a completly random terrain within the applications set parameters
    public void proceduralGenerate()
    {

        //Create random values 
        width = 1000;
        height = 1000;
        depth = randomInt(20, 100);
        scale = randomFloat(0.0001f, 50.00f);
        seed = randomInt(1, 10000);
        octaves = randomInt(1, 100);
        persistance = randomFloat(0.01f, 1.00f);
        lacunarity = randomFloat(0.01f, 20.00f);
        offsetX = randomFloat(0.00f, 500.00f);
        offsetY = randomFloat(0.00f, 500.00f);

        //Create the terrain in world
        GameObject generatedTerrain = Terrain.CreateTerrainGameObject(createTerrain.generateTerrain(width, height, depth, scale, seed, octaves, persistance, lacunarity, offsetX, offsetY));

        //Add texture to terrain 
        SplatPrototype[] terrainTexture = new SplatPrototype[1];
        terrainTexture[0].texture = (Texture2D)Resources.Load("MyTextures/PossibleRealGrass1");
    }

    // Get random value between X and Y
    private int randomInt(int min, int max)
    {
        return Random.Range(min, max);

    }

    private float randomFloat(float min, float max)
    {

        return Random.Range(min, max);

    }

}
