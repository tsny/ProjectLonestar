using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }
}
