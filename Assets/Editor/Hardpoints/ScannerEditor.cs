using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scanner))]
public class ScannerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Scanner scannerHardpoint = (Scanner)target;

        if (Application.isPlaying == false) return;

        if (GUILayout.Button("Scan"))
        {
            scannerHardpoint.ScanForTargets();
        }

        if (GUILayout.Button("Clear Entries"))
        {
            scannerHardpoint.ClearEntries();
        }
    }
} 