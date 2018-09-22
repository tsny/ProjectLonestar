using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HardpointSystem))]
public class HardpointSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HardpointSystem hardpointSystem = (HardpointSystem)target;

        if (Application.isPlaying == false) return;

        if (GUILayout.Button("Fire Active Weapons"))
        {
            hardpointSystem.FireActiveWeapons(new Vector3());
        }
    }
}