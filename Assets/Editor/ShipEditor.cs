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

        if (Application.isPlaying && GUILayout.Button("Die")) 
            ship.Die();

        ShowPossessButton();
    }

    private void ShowPossessButton()
    {
        var pc = GameSettings.pc;
        if (!Application.isPlaying) return;

        EditorGUI.BeginChangeCheck();

        var val = EditorGUILayout.Toggle("Is Possessed?", ship.Possessed);

        if (EditorGUI.EndChangeCheck())
        {
            if (val) pc.Possess(ship);
            else pc.Release();
        }
    }
}