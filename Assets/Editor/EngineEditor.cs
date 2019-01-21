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

        EGL.LabelField("Speed: " + engine.Speed);

        EGL.Space();
        ShowButtons(engine);
        EGL.Space();

        EGL.LabelField("Pitch: " + engine.Pitch);
        EGL.LabelField("Yaw: " + engine.Yaw);
        EGL.LabelField("Roll: " + engine.Roll);

        engine.Strafe = EGL.Slider("Strafe", engine.Strafe, -1, 1);
        engine.Throttle = EGL.Slider("Throttle", engine.Throttle, 0, 1);
    }

    [DrawGizmo(GizmoType.Selected)]
    static void ShowPredictedPath(Engine engine, GizmoType type)
    {
        if (!Application.isPlaying) return;
    }

    private void ShowButtons(Engine engine)
    {
        if (GUILayout.Button("Throttle Up"))
        {
            engine.ThrottleUp();
        }

        if (GUILayout.Button("Throttle Down"))
        {
            engine.ThrottleDown();
        }

        if (GUILayout.Button("Powerthrust Left"))
        {
            engine.SidestepHorizontal(false);
        }

        if (GUILayout.Button("Powerthrust Right"))
        {
            engine.SidestepHorizontal(true);
        }
        
        if (GUILayout.Button("Powerthrust Down"))
        {
            //engine.Sidestep();
        }
        
        if (GUILayout.Button("Powerthrust Up"))
        {
            //engine.Sidestep();
        }

        if (GUILayout.Button("Blink Forward"))
        {
            engine.Blink();
        }
    }
}