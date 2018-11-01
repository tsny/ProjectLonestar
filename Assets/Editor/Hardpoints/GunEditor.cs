using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;
using UnityEditor;

[CustomEditor(typeof(Gun))]
public class GunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Gun gun = (Gun)target;

        if (Application.isPlaying == false) return;

        gun.IsActive = EGL.Toggle("Active", gun.IsActive);

        if (GUILayout.Button("Fire"))
        {
            gun.Fire();
        }
    }
}