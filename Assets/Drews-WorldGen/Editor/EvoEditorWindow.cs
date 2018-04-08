using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
* The following class is the editor window for 
* 
**/
public class EvoEditorWindow : EditorWindow
{

    //Variables 
    public string evoType = "start";
    public int generation = 1; //Current Generation
    public List<TerrainSettings> population = new List<TerrainSettings>(); //Current Population
    public List<TerrainSettings> parents = new List<TerrainSettings>(); //Population to take on
    int terrainNumber;
    int populationToCreate = 10;


    void ShowWindow()
    {
        GetWindow<EvoEditorWindow>("GeneticEvolution");
    }

    void OnGUI()
    {

        GUILayout.Label("User Defined Generation", EditorStyles.centeredGreyMiniLabel);

        GUILayout.Label("Current Generation : " + generation, EditorStyles.centeredGreyMiniLabel);

        GUILayout.Label("Showing terrain " + terrainNumber + " / " + population.Count, EditorStyles.centeredGreyMiniLabel);

        GUILayout.Label("Parent's selected " + parents.Count + "(Minimal of 2)", EditorStyles.centeredGreyMiniLabel);

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

        //This button causes the complete random generation of a terrain
        if (GUILayout.Button("Optimal Solution"))
        {
            this.Close();
        }

        //This button causes the complete random generation of a terrain
        if (GUILayout.Button("Cancel Generation"))
        {
                DestroyImmediate(GameObject.Find("Terrain"));
                this.Close();
            

        }

        if (evoType == "start")
        {

            try
            {
                Debug.Log("User defined terrain generation started");

                EvoGeneration evoGeneration = new EvoGeneration();

                population = evoGeneration.createPopulation(populationToCreate);

                terrainNumber = 1;

                generation = 1;

                evoType = "next_gen";
            }

            catch (Exception exception)
            {
                this.Close();
                throw new ApplicationException("Terrain Generator has failed with the folloing exception : \n : ", exception);
            }

        }
        else if (evoType == "next_gen")
        {
            if (population.Count < terrainNumber)
            {


                population.Clear();

                generation = generation + 1;

                DestroyImmediate(GameObject.Find("Terrain"));

                GUILayout.Label("Showing terrain " + terrainNumber + " / " + population.Count, EditorStyles.centeredGreyMiniLabel);
                if (parents.Count < 2)
                {
                    Debug.LogError("At least 2 parents must be selected from the population, please run the tool again.");
                    if (GameObject.Find("Terrain") != null)
                    {
                        DestroyImmediate(GameObject.Find("Terrain"));
                        this.Close();
                    }

                }

                EvoGeneration evoGeneration = new EvoGeneration();

                population = evoGeneration.newGeneration(parents, populationToCreate);

                parents.Clear();

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

            }
        }
        else
        {
            this.Close();
        }
    }

}

