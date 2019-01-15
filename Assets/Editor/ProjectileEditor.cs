using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Projectile))]
public class ProjectileEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        Projectile projectile = (Projectile)target;
        base.OnInspectorGUI();

        if (Application.isPlaying && GUILayout.Button("Accelerate"))
        {
            projectile.Accelerate();
        }
    }
}