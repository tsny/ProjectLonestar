using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NebulaCameraFog))]
public class NebulaCamFogEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        var nebCam = target as NebulaCameraFog;

        EditorGUILayout.FloatField("Dist to nearest", nebCam.DistToNearestNeb);
    }
}