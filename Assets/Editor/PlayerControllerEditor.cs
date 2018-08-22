using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShipController))]
public class PlayerControllerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }
}
