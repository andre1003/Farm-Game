using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ResetSaves))]
public class ResetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ResetSaves resetSaves = (ResetSaves)target;
        
        // Reset player data and inventory button
        if(GUILayout.Button("Reset Player Data and Inventory"))
        {
            resetSaves.ResetAllPlayerData();
        }

        // Reset tutorials button
        if(GUILayout.Button("Reset Tutorials"))
        {
            resetSaves.ResetTutorials();
        }

        // Reset plantation zones button
        if(GUILayout.Button("Reset Plantation Zones"))
        {
            resetSaves.ResetPlantationZones();
        }

        // Reset game settings button
        if(GUILayout.Button("Reset Game Settings"))
        {
            resetSaves.ResetGameSettings();
        }

        // Reset all saves button
        if(GUILayout.Button("Reset All"))
        {
            resetSaves.ResetAll();
        }
    }
}
