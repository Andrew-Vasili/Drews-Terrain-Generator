using System;
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
    int populationToCreate = 10;
    bool randomGenerationCustomSettings = false;
    bool geneticGenerationCustomSettings = false;
    int worldSize = 100;
    GameObject objectToPlace;


    //Tool settings (default values in place
    Vector2 worldSizeSettings = new Vector2(100, 100);
    bool waterStatus = false;
    int worldSeed = 0;

    //Depth Values
    int depth = 0;
    int depthMin = 0;
    int depthMax = 50;

    //Scale Values
    float scale = 0;
    int scaleMin = 0;
    int scaleMax = 30;

    //Octaves Values
    int octaves = 0;
    int octavesMin = 0;
    int octavesMax = 10;

    //Persistance Values
    float persistance = 0;
    int persistanceMin = 0;
    int persistanceMax = 1;

    //Lacunarity Values
    float lacunarity = 0;
    int lacunarityMin = 0;
    int lacunarityMax = 20;


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

        GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Set mapsize to generate
        GUILayout.BeginHorizontal(GUILayout.Width(200));
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUILayout.Label("World Size, Width and Height");
        GUILayout.EndHorizontal();

        worldSize = EditorGUILayout.IntSlider(worldSize, 50, 2000, GUILayout.Width(200));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //This button causes the complete random generation of a terrain
        if (GUILayout.Button("Random Generation", GUILayout.Width(200)))
        {
            Debug.Log("Random generation started");
            try
            {
                if (randomGenerationCustomSettings == false)
                {
                    randomGeneration.proceduralGeneration(worldSize);
                }
                else
                {
                    randomGeneration.proceduralGenerationCustomSettings(worldSize, depth, scale, worldSeed, octaves, persistance, lacunarity, waterStatus);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Terrain Generator has failed with the folloing exception : \n : ", exception);
            }

            //----------------------------//
        }

        //This button causes the generation of terrain through the use of evolutionary algorithms 
        //----------------------------//

        if (GUILayout.Button("User Defined Generation", GUILayout.Width(200)))
        {
            if (geneticGenerationCustomSettings == true)
            {
                EvoEditorWindow inst = ScriptableObject.CreateInstance<EvoEditorWindow>();
                inst.worldSize = worldSize;
                inst.populationToCreate = populationToCreate;
                inst.Show();
            }
            else
            {
                EvoEditorWindow inst = ScriptableObject.CreateInstance<EvoEditorWindow>();
                inst.worldSize = worldSize;
                inst.populationToCreate = 10;
                inst.Show();
            }

        }
        if (GameObject.Find("Terrain") != null)
        {

            objectToPlace = EditorGUILayout.ObjectField("ObjectToPlace", objectToPlace, typeof(GameObject)) as GameObject;

            //This button causes the complete random generation of a terrain
            if (GUILayout.Button("Spawn Object", GUILayout.Width(200)))
            {
                Debug.Log("Object Spawning Started");
                try
                {
                    Terrain terrain = Terrain.activeTerrain;
                    int numberOfObjectsToPlace = 100; // Number of objects to place
                    GameObject objectToPlace = Resources.Load("Trees/" + "Tree1") as GameObject; // Object to place
                    int terrainWidth; // terrain size (x)
                    int terrainLength; // terrain size (z)
                    int terrainPosX; // terrain position x
                    int terrainPosZ; // terrain position z

                    // terrain width
                    terrainWidth = (int)terrain.terrainData.size.x;
                    // terrain length
                    terrainLength = (int)terrain.terrainData.size.z;

                    // terrain positions
                    terrainPosX = (int)terrain.transform.position.x;
                    terrainPosZ = (int)terrain.transform.position.z;

                    // generate objects
                    for (int x = 0; x <= numberOfObjectsToPlace; x++)
                    {
                        // generate random position for placement then get height of positon
                        int positionX = UnityEngine.Random.Range(terrainPosX, terrainPosX + terrainWidth);
                        int positionZ = UnityEngine.Random.Range(terrainPosZ, terrainPosZ + terrainLength);
                        float positionY = Terrain.activeTerrain.SampleHeight(new Vector3(positionX, 0, positionZ));

                        // create new gameObject on random position
                        GameObject worldObject = Instantiate(objectToPlace, new Vector3(positionX, positionY, positionZ), Quaternion.identity);
                    }
                 
                }
                catch (Exception exception)
                {
                    throw new ApplicationException("Terrain Generator has failed with the folloing exception : \n : ", exception);
                }
            }
            //----------------------------//
        }

        GUILayout.EndVertical();

        //----------------------------//

        //This button shows the settings of the tool
        //----------------------------//
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        randomGenerationCustomSettings = EditorGUILayout.BeginToggleGroup("Random Generation Settings", randomGenerationCustomSettings);

        if (randomGenerationCustomSettings)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            waterStatus = EditorGUILayout.Toggle("Water Toggle", waterStatus, GUILayout.Width(200));

            worldSeed = EditorGUILayout.IntField("World worldSeed", worldSeed, GUILayout.Width(200));

            //Octaves Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Noise Map Layers = " + octaves);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            octaves = Mathf.RoundToInt(GUILayout.HorizontalSlider(octaves, octavesMin, octavesMax, GUILayout.Width(200)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.Label(octavesMin.ToString());
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label(octavesMax.ToString());
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            //-------------------------------------------------------------------------------//

            //Depth Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Height of Terrain = " + depth);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            depth = Mathf.RoundToInt(GUILayout.HorizontalSlider(depth, depthMin, depthMax, GUILayout.Width(200)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.Label(depthMin.ToString());
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label(depthMax.ToString());
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            //-------------------------------------------------------------------------------//

            //Scale Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Scale = " + scale);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            scale = GUILayout.HorizontalSlider(scale, scaleMin, scaleMax, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.Label(scaleMin.ToString());
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label(scaleMax.ToString());
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            //-------------------------------------------------------------------------------//


            //Lacunarity Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Bumpiness = " + lacunarity);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            lacunarity = GUILayout.HorizontalSlider(lacunarity, lacunarityMin, lacunarityMax, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.Label(lacunarityMin.ToString());
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label(lacunarityMax.ToString());
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            //-------------------------------------------------------------------------------//


            //Persistance Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Bumpiness occurence = " + persistance);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            persistance = GUILayout.HorizontalSlider(persistance, persistanceMin, persistanceMax, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.Label(persistanceMin.ToString());
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label(persistanceMax.ToString());
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            //-------------------------------------------------------------------------------//


        }
        EditorGUILayout.EndToggleGroup();
        //----------------------------//

        EditorGUILayout.Space();
        geneticGenerationCustomSettings = EditorGUILayout.BeginToggleGroup("Genetic Generation Settings", geneticGenerationCustomSettings);

        if (geneticGenerationCustomSettings)
        {
            //Population Size Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Population per generation = " + populationToCreate);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            populationToCreate = Mathf.RoundToInt(GUILayout.HorizontalSlider(populationToCreate, 10, 50, GUILayout.Width(200)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(200));
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.Label(10.ToString());
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label(50.ToString());
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            //-------------------------------------------------------------------------------//
        }
        EditorGUILayout.EndToggleGroup();
    }
}

