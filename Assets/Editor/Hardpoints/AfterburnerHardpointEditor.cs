using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AfterburnerHardpoint))]
public class AfterburnerHardpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AfterburnerHardpoint afterburnerHardpoint = (AfterburnerHardpoint)target;

        EditorGUI.BeginChangeCheck();

            Afterburner newAfterburner = (Afterburner) EditorGUILayout.ObjectField("Afterburner", afterburnerHardpoint.CurrentEquipment, typeof(Afterburner), true);

        if (EditorGUI.EndChangeCheck())
        {
            if (newAfterburner == null)
            {
                afterburnerHardpoint.Demount();
            }

            afterburnerHardpoint.TryMount(newAfterburner);
        }

        if (afterburnerHardpoint.IsMounted)
        {
            if (GUILayout.Button("Demount"))
            {
                afterburnerHardpoint.Demount();
            }
        }
    }
}