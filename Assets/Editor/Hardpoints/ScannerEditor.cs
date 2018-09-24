using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scanner))]
public class ScannerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Scanner scanner = (Scanner)target;

        if (Application.isPlaying == false) return;

        if (GUILayout.Button("Scan"))
        {
            scanner.ScanForTargets();
        }

        if (GUILayout.Button("Clear Entries"))
        {
            scanner.ClearList();
        }
    }
} 