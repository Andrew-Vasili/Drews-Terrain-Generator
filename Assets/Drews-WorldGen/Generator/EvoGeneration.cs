using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EvoGeneration : EditorWindow
{

    TerrainSettings terrainSettings = new TerrainSettings();
    CreateTerrain createTerrain = new CreateTerrain();

    public int Generation { get; private set; } //Current Generation

    private List<TerrainSettings> population = new List<TerrainSettings>(); //Current Population

    //Begin the process 
    public void startGA()
    {
        //Setup starting varaibles
        Generation = 1;

        for (int x = 0; x < 10;)
        {
            Debug.Log("Creating Values");

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

            Debug.Log("Creating population : " + x);

            //Create terrain settings object
            terrainSettings.setupTerrain(width, height, depth, scale, seed, octaves, persistance, lacunarity, offsetX, offsetY);

            population.Add(terrainSettings);

            x++;
        }




    }

    int test()
    {
        int x = 1;
        return x;
    }

    static void Init()
    {
        EvoGeneration window = ScriptableObject.CreateInstance<EvoGeneration>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("This is an example of EditorWindow.ShowPopup", EditorStyles.wordWrappedLabel);
        GUILayout.Space(70);
        if (GUILayout.Button("Agree!"))
        {
            this.Close();
        }



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
