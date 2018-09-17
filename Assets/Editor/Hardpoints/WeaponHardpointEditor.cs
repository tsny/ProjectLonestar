using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponHardpoint))]
public class WeaponHardpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponHardpoint weaponHardpoint = (WeaponHardpoint)target;

        EditorGUI.BeginChangeCheck();

            Weapon newWeapon = (Weapon) EditorGUILayout.ObjectField("Weapon", weaponHardpoint.CurrentEquipment, typeof(Weapon), true);

        if (EditorGUI.EndChangeCheck())
        {
            if (newWeapon == null)
            {
                weaponHardpoint.Demount();
            }

            weaponHardpoint.TryMount(newWeapon);
        }

        if (weaponHardpoint.IsMounted)
        {
            if (GUILayout.Button("Demount"))
            {
                weaponHardpoint.Demount();
            }
        }
    }
}