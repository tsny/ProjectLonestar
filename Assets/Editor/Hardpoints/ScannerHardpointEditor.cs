using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScannerHardpoint))]
public class ScannerHardpointEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ScannerHardpoint scannerHardpoint = (ScannerHardpoint)target;

        EditorGUI.BeginChangeCheck();

            Scanner newScanner = (Scanner) EditorGUILayout.ObjectField("Scanner", scannerHardpoint.CurrentEquipment, typeof(Scanner), true);

        if (EditorGUI.EndChangeCheck())
        {
            if (newScanner == null)
            {
                scannerHardpoint.Demount();
            }

            scannerHardpoint.TryMount(newScanner);
        }

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