using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fire))]
public class FireEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        var fire = target as Fire;

        if (GUILayout.Button("Fire"))
        {
            fire.FireThis();
        } 

        if (!Application.isPlaying) return;

        if (GUILayout.Button("Fire ALL"))
        {
            fire.FireAll();
        } 
    }
}