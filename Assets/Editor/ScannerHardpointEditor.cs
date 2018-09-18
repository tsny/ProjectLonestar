using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScannerHardpoint))]
public class ScannerHardpointEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ScannerHardpoint scanner = (ScannerHardpoint)target;

        if (GUILayout.Button("Scan"))
        {
            scanner.Scan();
        }

        if (GUILayout.Button("Clear Entries"))
        {
            scanner.ClearEntries();
        }
    }
} 