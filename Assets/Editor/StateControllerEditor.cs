using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateController))]
public class StateControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var cont = target as StateController;

        GUILayout.Space(10);

        if (GUILayout.Button("Target Random Object"))
        {
            var targets = FindObjectsOfType<MeshRenderer>();
            cont.targetTrans = targets[UnityEngine.Random.Range(0, targets.Length)].gameObject.transform;
        }

        if (GUILayout.Button("Target Random Ship"))
        {
            var targets = FindObjectsOfType<Ship>();
            cont.targetTrans = targets[UnityEngine.Random.Range(0, targets.Length)].gameObject.transform;
        }

        if (GUILayout.Button("Target Player Ship"))
        {
            cont.targetTrans = GameSettings.pc.ship.transform;
        }

        if (GUILayout.Button("Clear Target"))
        {
            cont.targetTrans = null;
        }

        if (GUILayout.Button("Clear Current State"))
        {
            cont.currentState = null;
        }

        if (GUILayout.Button("Toggle AI"))
        {
            cont.aiIsActive = !cont.aiIsActive;
        }
    }
}