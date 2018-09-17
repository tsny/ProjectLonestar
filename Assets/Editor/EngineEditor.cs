using UnityEngine;
using UnityEditor;
using EGL = UnityEditor.EditorGUILayout;

[CustomEditor(typeof(Engine))]
public class EngineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var engine = target as Engine;

        if (Application.isPlaying == false) return;

        EGL.Space();

        if (GUILayout.Button("Throttle Up"))
        {
            engine.ThrottleUp();
        }

        if (GUILayout.Button("Throttle Down"))
        {
            engine.ThrottleDown();
        }

        EGL.Space();

        engine.Strafe = EGL.Slider("Strafe", engine.Strafe, -1, 1);
        engine.Throttle = EGL.Slider("Throttle", engine.Throttle, 0, 1);
    }
}