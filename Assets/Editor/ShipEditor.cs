using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ship))]
public class ShipEditor : Editor
{
    Ship ship;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ship = target as Ship;

        EditorGUI.BeginChangeCheck();

        var newBase = (ShipBase) EditorGUILayout.ObjectField("ShipBase", ship.ShipBase, typeof(ShipBase), false);

        if (EditorGUI.EndChangeCheck())
        {
            ship.Init(newBase);
        }

        if (Application.isPlaying && GUILayout.Button("Die")) 
            ship.Die();

        if (Application.isPlaying)
            EditorGUILayout.Toggle("Is Possessed?", ship.Possessed);

        ShowPossessButton();
        ShowPossessionHandle();
    }

    private void ShowPossessionHandle()
    {
        if (!Application.isPlaying || GameSettings.pc.ship == ship) return;

        if (Handles.Button(ship.transform.position + Vector3.up * 10, Quaternion.identity, 3, 3, Handles.SphereHandleCap))
            GameSettings.pc.Possess(ship);
    }

    private void ShowPossessButton()
    {
        var pc = GameSettings.pc;

        if (!Application.isPlaying) return;

        if (pc.ship != ship)
        {
            if (GUILayout.Button("Possess"))
                pc.Possess(ship);
        }

        else
        {
            if (GUILayout.Button("Unpossess"))
                pc.Release();
        }
    }
}