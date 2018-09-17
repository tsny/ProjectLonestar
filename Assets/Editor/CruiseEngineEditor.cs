using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CruiseEngine))]
public class CruiseEngineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var engine = target as CruiseEngine;

        EditorGUILayout.LabelField("Cruise State", engine.State.ToString());

        if (Application.isPlaying && GUILayout.Button("Toggle Engines"))
        {
            engine.ToggleCruiseEngines();
        }

        Repaint();
    }
}
