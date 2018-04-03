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
    public string evoType = "start";
    public int generation = 1; //Current Generation
    public List<TerrainSettings> population = new List<TerrainSettings>(); //Current Population
    public List<TerrainSettings> parents = new List<TerrainSettings>(); //Population to take on
    int terrainNumber;
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
                try
                {
                    Debug.Log("User defined terrain generation started");

                    EvoGeneration evoGeneration = new EvoGeneration();

                    population = evoGeneration.createPopulation(populationToCreate);

                    terrainNumber = 1;

                    generation = 1;

                    menuType = "Evo";
                }
                catch (Exception exception)
                {
                    menuType = "menu";
                    throw new ApplicationException("Terrain Generator has failed with the folloing exception : \n : ", exception);
                }
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
                GUILayout.Label("User Defined Generation", EditorStyles.centeredGreyMiniLabel);

                GUILayout.Label("Current Generation : " + generation, EditorStyles.centeredGreyMiniLabel);

                GUILayout.Label("Showing terrain " + terrainNumber + " / " + population.Count, EditorStyles.centeredGreyMiniLabel);

                GUILayout.Label("Parent's selected " + parents.Count + "(Minimal of 2)", EditorStyles.centeredGreyMiniLabel);



                {
                    if (population.Count < terrainNumber)
                    {
                        generation = generation + 1;

                        DestroyImmediate(GameObject.Find("Terrain"));

                        GUILayout.Label("Showing terrain " + terrainNumber + " / " + population.Count, EditorStyles.centeredGreyMiniLabel);
                        if (parents.Count < 2)
                        {
                            Debug.LogError("At least 2 parents must be selected from the population, please run the tool again.");
                            if (GameObject.Find("Terrain") != null)
                            {
                                DestroyImmediate(GameObject.Find("Terrain"));
                            }

                            menuType = "menu";
                        }

                        EvoGeneration evoGeneration = new EvoGeneration();

                        population = evoGeneration.newGeneration(parents, populationToCreate);

                        terrainNumber = 1;

                    }

                    else
                    {


                        if (GameObject.Find("Terrain") != null)
                        {
                        }
                        else
                        {
                            CreateTerrain createTerrain = new CreateTerrain();
                            GameObject generatedTerrain = Terrain.CreateTerrainGameObject(createTerrain.generateTerrain(population[terrainNumber - 1]));
                        }

                        if (GUILayout.Button("Approve"))
                        {

                            parents.Add(population[terrainNumber - 1]);

                            terrainNumber = terrainNumber + 1;
                            DestroyImmediate(GameObject.Find("Terrain"));

                        }

                        if (GUILayout.Button("Decline"))
                        {
                            DestroyImmediate(GameObject.Find("Terrain"));
                            terrainNumber = terrainNumber + 1;

                        }
                    }
                }


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
                evoType = "start";

            }
        }
        else
        {
            Debug.Log("Broke");
        }
    }
}
