using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TractorBeam))]
public class TractorBeamEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TractorBeam tractor = (TractorBeam)target;

    }
}