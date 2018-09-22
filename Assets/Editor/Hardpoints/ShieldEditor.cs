using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Shield))]
public class ShieldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Shield shieldHardpoint = (Shield)target;

    }
}