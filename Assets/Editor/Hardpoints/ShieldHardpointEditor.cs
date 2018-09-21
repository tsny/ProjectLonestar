using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShieldHardpoint))]
public class ShieldHardpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShieldHardpoint shieldHardpoint = (ShieldHardpoint)target;

    }
}