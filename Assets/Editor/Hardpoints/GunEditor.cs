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

        //Ray ray = new Ray()
        Vector3 projectileEndPoint = gun.transform.forward * gun.projectile.stats.range;
        Debug.DrawRay(gun.SpawnPoint, projectileEndPoint, Color.green);
        //Gizmos.DrawWireSphere(field.transform.position, field.outerRadius);
        //Gizmos.DrawWireSphere(field.transform.position, field.innerRadius);
    }
}