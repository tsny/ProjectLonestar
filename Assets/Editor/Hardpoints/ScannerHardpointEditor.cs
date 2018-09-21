using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScannerHardpoint))]
public class ScannerHardpointEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ScannerHardpoint scannerHardpoint = (ScannerHardpoint)target;

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