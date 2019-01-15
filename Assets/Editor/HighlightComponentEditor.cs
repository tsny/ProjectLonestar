using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HighlightComponent))]
public class HighlightComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var hc = target as HighlightComponent;

        if (!Application.isPlaying) return;

        if (GUILayout.Button("Toggle Shader"))
        {
            hc.ToggleShader(0);
        }
    }
}