using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SettingsWindow : EditorWindow
{

    SettingsWindow settings = new SettingsWindow();


    void ShowWindow()
    {
        GetWindow<SettingsWindow>("Settings");
    }

    void OnGUI()
    {

        GUILayout.Label("Settings", EditorStyles.centeredGreyMiniLabel);
    }
}
