using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateController))]
public class StateControllerEditor : Editor
{
    StateController cont;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        cont = target as StateController;
        if (cont.HasTarget) EditorGUILayout.LabelField("Distance To Target: " + cont.DistanceToTarget);
        GUILayout.Space(10);
        ShowButtons();
    }

    private void ShowButtons()
    {
        if (Btn("Target Random GameObject w/ Mesh"))
        {
            var targets = FindObjectsOfType<MeshRenderer>();
            cont.Target = targets[UnityEngine.Random.Range(0, targets.Length)].gameObject;
        }

        if (Btn("Target Random Enemy"))
        {
            cont.TargetRandomEnemy();
        }

        if (Btn("Target Player Ship"))
            cont.Target = PlayerController.Instance.ship.gameObject;

        if (Btn("Clear Target"))
            cont.Target = null;

        if (Btn("Reset AI"))
            cont.ResetAI();

        // Play-mode buttons

        if (Application.isPlaying && Btn("Toggle AI"))
            cont.aiIsActive = !cont.aiIsActive;

        if (Application.isPlaying && Btn("Full Stop"))
        {
            cont.pastStates.Clear();
            cont.currentState = cont.stopState;
        }
    }

    private void OnSceneGUI() 
    {
        if (!cont) return;
        if (cont.HasTarget) Debug.DrawLine(cont.transform.position, cont.TargetTransform.position, Color.cyan);
    }

    private bool Btn(String str)
    {
        return GUILayout.Button(str);
    }
}