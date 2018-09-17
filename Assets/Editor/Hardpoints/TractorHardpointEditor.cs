using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TractorHardpoint))]
public class TractorHardpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TractorHardpoint tractor = (TractorHardpoint)target;

        EditorGUI.BeginChangeCheck();

            TractorBeam newTractor = (TractorBeam) EditorGUILayout.ObjectField("Tractor", tractor.CurrentEquipment, typeof(TractorBeam), true);

        if (EditorGUI.EndChangeCheck())
        {
            if (newTractor == null)
            {
                tractor.Demount();
            }

            tractor.TryMount(newTractor);
        }

        if (tractor.IsMounted)
        {
            if (GUILayout.Button("Demount"))
            {
                tractor.Demount();
            }
        }
    }
}