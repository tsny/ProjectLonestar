using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Projectile))]
public class ProjectileEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        Projectile projectile = (Projectile)target;
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Distance Traveled: " + projectile.DistanceTraveled);

        if (Application.isPlaying && GUILayout.Button("Accelerate"))
        {
            projectile.Accelerate();
        }
    }
}