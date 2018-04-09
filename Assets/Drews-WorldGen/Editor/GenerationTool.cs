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
    bool customSettings = false;
    int worldSize = 100;

    //Tool settings (default values in place
    Vector2 worldSizeSettings = new Vector2(100, 100);
    bool waterStatus = false;
    int seed = 0;

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
    float persistanceMin = 0.00f;
    float persistanceMax = 1.00f;

    //Lacunarity Values
    float lacunarity = 0;
    float lacunarityMin = 0.00f;
    float lacunarityMax = 20.00f;


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
                if (customSettings == false)
                {
                    randomGeneration.proceduralGeneration(worldSize);
                }
                else
                {
                    randomGeneration.proceduralGenerationCustomSettings(worldSize, depth, scale, seed, octaves, persistance, lacunarity, waterStatus);
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
            EvoEditorWindow inst = ScriptableObject.CreateInstance<EvoEditorWindow>();
            inst.Show();

        }

        GUILayout.EndVertical();

        //----------------------------//

        //This button shows the settings of the tool
        //----------------------------//
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        customSettings = EditorGUILayout.BeginToggleGroup("Enable Custom Settings", customSettings);

        if (customSettings)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            waterStatus = EditorGUILayout.Toggle("Water Toggle", waterStatus, GUILayout.Width(200));

            seed = EditorGUILayout.IntField("World Seed", seed, GUILayout.Width(200));

            //Octaves Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Octaves = " + octaves);
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
            GUILayout.Label("Depth = " + depth);
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

            //Persistance Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Persistance = " + persistance);
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

            //Lacunarity Slider
            //-------------------------------------------------------------------------------//
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Lacunarity = " + lacunarity);
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

            GUILayout.EndVertical();
        }
        EditorGUILayout.EndToggleGroup();
        //----------------------------//
    }
}

