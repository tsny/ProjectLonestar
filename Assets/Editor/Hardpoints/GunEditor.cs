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

        gun.IsActive = EGL.Toggle("Active", gun.IsActive);

        if (GUILayout.Button("Fire"))
        {
            gun.Fire(null);
        }
    }
}