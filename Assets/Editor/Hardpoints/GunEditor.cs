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

        if (Application.isPlaying && GUILayout.Button("Fire"))
        {
            gun.Fire(null);
        }
    }

    [DrawGizmo(GizmoType.Selected)]
    static void ShowProjectilePath(Gun gun, GizmoType type)
    {
        if (gun.projectile == null) return;
        if (gun.rbTarget == null) return;
        var pos = Utilities.CalculateAimPosition(gun.SpawnPoint, gun.rbTarget, gun.stats.thrust);
        Gizmos.DrawWireSphere(pos, 3);
    }
}