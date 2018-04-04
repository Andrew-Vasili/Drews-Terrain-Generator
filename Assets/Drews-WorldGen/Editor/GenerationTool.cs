using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * This class controls the Unity tools editor window
 **/

[CustomEditor(typeof(GenerationTool))]

public class GenerationTool : EditorWindow
{

    //Classes set
    public RandomGeneration randomGeneration = new RandomGeneration();
    public TerrainSettings terrainSettings = new TerrainSettings();
    public CreateTerrain createTerrain = new CreateTerrain();

    //Variables for menu
    public string menuType = "";
    int populationToCreate = 10;

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
        GUILayout.Label("Drews Terrain Generator", EditorStyles.centeredGreyMiniLabel);

        //Main Menu
        //----------------------------//
        if (menuType == "menu")
        {

            GUILayout.Label("Main Menu", EditorStyles.centeredGreyMiniLabel);

            //This button causes the complete random generation of a terrain
            if (GUILayout.Button("Complete Random Generation"))
            {
                Debug.Log("Random generation started");
                try
                {
                    randomGeneration.proceduralGenerate();
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
            if (GUILayout.Button("Settings"))
            {
                Debug.Log("Settings menu selected");

                menuType = "settings";

            }
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



        else if (menuType != "menu")
        {
            //This button causes the complete random generation of a terrain
            if (GUILayout.Button("Reset tool"))
            {
                if (GameObject.Find("Terrain") != null)
                {
                    DestroyImmediate(GameObject.Find("Terrain"));
                }
                menuType = "menu";

            }
        }
        else
        {
            Debug.Log("Broke");
        }
    }
}
