using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TargetIndicator))]
public class TargetIndicatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TargetIndicator targ = (TargetIndicator) target;
        EditorGUILayout.Vector3Field("TargetViewportPoint", targ.TargetViewportPoint);
        EditorGUILayout.Toggle("Is On Screen", targ.TargetIsOnScreen);
        EditorGUILayout.Toggle("In Range", targ.TargetIsInRange);
    }
}