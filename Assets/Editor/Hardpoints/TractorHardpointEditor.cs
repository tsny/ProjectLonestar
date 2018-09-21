using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TractorHardpoint))]
public class TractorHardpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TractorHardpoint tractor = (TractorHardpoint)target;

    }
}