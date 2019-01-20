using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        var hp = target as Health;

        hp.Invulnerable = EditorGUILayout.Toggle("Invulnerable", hp.Invulnerable);
    }
}