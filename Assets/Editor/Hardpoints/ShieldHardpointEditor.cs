using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShieldHardpoint))]
public class ShieldHardpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShieldHardpoint shieldHardpoint = (ShieldHardpoint)target;

        EditorGUI.BeginChangeCheck();

        Shield newShield = (Shield) EditorGUILayout.ObjectField("Shield", shieldHardpoint.CurrentEquipment, typeof(Shield), true);

        if (EditorGUI.EndChangeCheck())
        {
            if (newShield == null)
            {
                shieldHardpoint.Demount();
            }

            shieldHardpoint.TryMount(newShield);
        }

        if (shieldHardpoint.IsMounted)
        {
            if (GUILayout.Button("Demount"))
            {
                shieldHardpoint.Demount();
            }
        }
    }
}