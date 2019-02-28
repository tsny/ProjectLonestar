using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Emitter))]
public class EmitterEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        var emitter = target as Emitter;
        base.OnInspectorGUI();
        if (emitter.ps && GUILayout.Button("Fire"))
        {
            emitter.Fire();
        }
    }
}