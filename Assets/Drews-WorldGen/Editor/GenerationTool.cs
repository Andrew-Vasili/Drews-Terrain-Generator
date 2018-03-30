using UnityEngine;
using UnityEditor;

/**
 * This class controls the Unity tools editor window
 **/

[CustomEditor(typeof(GenerationTool))]

public class GenerationTool : EditorWindow
{

    //Setup window view
    [MenuItem("Window/Drews Terrain Generator")]
    public static void ShowWindow()
    {
        GetWindow<GenerationTool>("Drews Terrain Generator");
    }

    // Window Contents
    void OnGUI()
    {

        //Classes set
        EvoGeneration evoGeneration = new EvoGeneration();
        RandomGeneration randomGeneration = new RandomGeneration();

        //Application name 
        GUILayout.Label("Drews Terrain Generator", EditorStyles.boldLabel);

        //This button causes the complete random generation of a terrain
        if (GUILayout.Button("Complete Random Generation"))
        {
            Debug.Log("Drew's Test");

            randomGeneration.proceduralGenerate();

            Debug.Log("Drew's Test Complete");
        }

        //This button causes the generation of terrain through the use of evolutionary algorithms 
        if (GUILayout.Button("User Defined Generation"))
        {
            evoGeneration.startGA();
            Debug.Log("Test Complete");

        }

        //This button shows the settings of the tool
        if (GUILayout.Button("Settings"))
        {
            Debug.Log("Test Complete");
        }

    }

}
