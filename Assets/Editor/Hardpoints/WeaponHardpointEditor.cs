using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;
using UnityEditor;

[CustomEditor(typeof(WeaponHardpoint))]
public class WeaponHardpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponHardpoint weaponHardpoint = (WeaponHardpoint)target;

        if (Application.isPlaying == false) return;

        weaponHardpoint.Active = EGL.Toggle("Active", weaponHardpoint.Active);

        if (GUILayout.Button("Fire"))
        {
            //weaponHardpoint.Fire();
        }
    }
}