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
    bool showSettings = false;
    String customSettings = "disabled";
    int worldSize = 100;

    //Tool settings (default values in place
    Vector2 worldSizeSettings = new Vector2(100, 100);
    bool waterStatus = false;
    int Depth;
    float Scale;
    int Seed;
    int Octaves;
    float Persistance;
    float Lacunarity;
    float OffsetX;
    float OffsetY;

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
            GUILayout.Label("World Size, Width and Height", EditorStyles.centeredGreyMiniLabel);
            worldSize = EditorGUILayout.IntSlider(worldSize, 50, 2000);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //This button causes the complete random generation of a terrain
            if (GUILayout.Button("Random Generation"))
            {
                Debug.Log("Random generation started");
                try
                {
                    randomGeneration.proceduralGenerate(worldSize);
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

            if (GUILayout.Button("User Defined Generation"))
            {
                EvoEditorWindow inst = ScriptableObject.CreateInstance<EvoEditorWindow>();
                inst.Show();

            }
            //----------------------------//





            //This button shows the settings of the tool
            //----------------------------//
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            showSettings = EditorGUILayout.BeginToggleGroup("Custom Settings Enabled : " + showSettings, showSettings);

            if (showSettings)
            {

                customSettings = "Enabled";
                GUILayout.Label("Water enabled = " + waterStatus);
                if(GUILayout.Button("Change Water Status"))
                {
                    if (waterStatus)
                    {
                        waterStatus = false;
                    }
                    else
                    {
                        waterStatus = true;
                    }
                    
                }

                worldSizeSettings = EditorGUILayout.Vector2Field("World Size (Width, Height)", worldSizeSettings);


            }
            EditorGUILayout.EndToggleGroup();
            //----------------------------//



        }

        //Settings of application
        else if (menuType == "settings")
        {
            GUILayout.Label("Settings", EditorStyles.centeredGreyMiniLabel);

        }

        else if (menuType == "Evo")
        {

            try
            {
              


            }
            catch (Exception exception)
            {
                menuType = "menu";
                throw new ApplicationException("Terrain Generator has failed with the folloing exception : \n : ", exception);
            }
        }
        else
        {
            Debug.Log("Broke");
        }
    }

}
