using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShipSpawner))]
public class ShipSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var spawner = target as ShipSpawner;

        if (!Application.isPlaying) return;

        if (GUILayout.Button("Spawn Ships"))
        {
            spawner.TriggerSelf();
        }
    }
}
