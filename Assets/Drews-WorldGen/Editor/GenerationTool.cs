using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * Master class for the entire tool
 **/

[CustomEditor(typeof(GenerationTool))]

public class GenerationTool : EditorWindow
{

    //Classes set
    public RandomGeneration randomGeneration = new RandomGeneration();
    public TerrainSettings terrainSettings = new TerrainSettings();
    public CreateTerrain createTerrain = new CreateTerrain();

    //Starting variables for menu
    public string menuType = "menu";
    int populationToCreate = 10;
    bool customSettings = false;
    int worldSize = 100;

    //Tool settings (default values in place
    Vector2 worldSizeSettings = new Vector2(100, 100);
    bool waterStatus = false;
    int depth = 0;
    float scale = 0;
    int seed = 0;
    int octaves = 0;
    float persistance = 0;
    float lacunarity = 0;
    float offsetX = 0;
    float offsetY = 0;

    //Setup window view
    [MenuItem("Window/Drews Terrain Generator")]
    public static void ShowWindow()
    {
        GetWindow<GenerationTool>("Drews Terrain Generator");

    }

    // Window Contents
    void OnGUI()
    {

        //Application name 
        GUILayout.Label("Drews Terrain Generator", EditorStyles.boldLabel);

        //Main Menu
        //----------------------------//
        if (menuType == "menu")
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //Set mapsize to generate
            GUILayout.Label("World Size, Width and Height");
            worldSize = EditorGUILayout.IntSlider(worldSize, 50, 2000, GUILayout.Width(200));

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //This button causes the complete random generation of a terrain
            if (GUILayout.Button("Random Generation", GUILayout.Width(200)))
            {
                Debug.Log("Random generation started");
                try
                {
                    if (customSettings == false)
                    {
                        randomGeneration.proceduralGeneration(worldSize);
                    }
                    else
                    {
                        randomGeneration.proceduralGenerationCustomSettings(worldSize, depth, scale, seed, octaves, persistance, lacunarity, offsetX, offsetY, waterStatus);
                    }
                }
                catch (Exception exception)
                {
                    menuType = "menu";
                    throw new ApplicationException("Terrain Generator has failed with the folloing exception : \n : ", exception);
                }
            }
            //----------------------------//


            //This button causes the generation of terrain through the use of evolutionary algorithms 
            //----------------------------//

            if (GUILayout.Button("User Defined Generation", GUILayout.Width(200)))
            {
                EvoEditorWindow inst = ScriptableObject.CreateInstance<EvoEditorWindow>();
                inst.Show();

            }
            //----------------------------//

            //This button shows the settings of the tool
            //----------------------------//
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            customSettings = EditorGUILayout.BeginToggleGroup("Custom Settings Enabler", customSettings);

            if (customSettings)
            {

                waterStatus = EditorGUILayout.Toggle("Water Toggle", waterStatus, GUILayout.Width(200));

                seed = EditorGUILayout.IntField("Seed", seed, GUILayout.Width(200));

                octaves = EditorGUILayout.IntField("Octaves", octaves, GUILayout.Width(200));

                seed = EditorGUILayout.IntField("Seed", seed, GUILayout.Width(200));

                depth = EditorGUILayout.IntField("Depth", depth, GUILayout.Width(200));

                scale = EditorGUILayout.FloatField("Scale", scale, GUILayout.Width(200));

                persistance = EditorGUILayout.FloatField("Persistance", persistance, GUILayout.Width(200));

                lacunarity = EditorGUILayout.FloatField("Lacunarity", lacunarity, GUILayout.Width(200));

                offsetY = EditorGUILayout.FloatField("Offset Y", offsetY, GUILayout.Width(200));

                offsetX = EditorGUILayout.FloatField("Offset X", offsetX, GUILayout.Width(200));

            }
            EditorGUILayout.EndToggleGroup();
            //----------------------------//



        }

        else
        {
            Debug.Log("Broke");
        }

    }
}

