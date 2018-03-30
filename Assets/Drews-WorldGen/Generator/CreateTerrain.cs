using UnityEditor;
using UnityEngine;

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

    //Generate a terrain method
    public TerrainData generateTerrain(int widthPre, int heightPre, int depthPre, float scalePre, int seedPre, int octavesPre, float persistancePre, float lacunarityPre, float offsetXPre, float offsetYPre)
    {

        //Setters
        width = widthPre;
        height = heightPre;
        depth = depthPre;
        scale = scalePre;
        seed = seedPre;
        octaves = octavesPre;
        persistance = persistancePre;
        lacunarity = lacunarityPre;
        offsetX = offsetXPre;
        offsetY = offsetYPre;

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
        terrainData = GenerateHeightMap(terrainData);
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

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseX = x / scale;
                float noiseY = y / scale;

                float perlinValue = Mathf.PerlinNoise(noiseX, noiseY);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }

    // Get random value between X and Y
    int randomValue(int min, int max)
    {
        int valueToReturn = Random.Range(min, max);
        return valueToReturn;
    }
}