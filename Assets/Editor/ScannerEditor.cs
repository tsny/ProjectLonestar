using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScannerHardpoint))]
public class ScannerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        ScannerHardpoint scanner = (ScannerHardpoint)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Scan"))
        {
            scanner.Scan();
        }

        if (GUILayout.Button("Clear Entries"))
        {
            scanner.detectedObjects.Clear();
        }
    }
} 