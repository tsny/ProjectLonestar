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

        if (Application.isPlaying == false) return;

        EditorGUILayout.LabelField("Cruise State", engine.State.ToString());

        if (GUILayout.Button("Toggle Engines"))
        {
            engine.ToggleCruiseEngines();
        }

        Repaint();
    }
}
