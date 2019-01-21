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

        if (Btn("Target Random Object"))
        {
            var targets = FindObjectsOfType<MeshRenderer>();
            cont.targetTrans = targets[UnityEngine.Random.Range(0, targets.Length)].gameObject.transform;
        }

        if (Btn("Target Random Ship"))
        {
            var targets = FindObjectsOfType<Ship>();
            cont.targetTrans = targets[UnityEngine.Random.Range(0, targets.Length)].gameObject.transform;
        }

        if (Btn("Target Player Ship"))
        {
            cont.targetTrans = GameSettings.pc.ship.transform;
        }

        if (Btn("Clear Target"))
        {
            cont.targetTrans = null;
        }

        if (Btn("Clear Current State"))
        {
            cont.pastStates.Clear();
            cont.currentState = null;
        }

        if (Btn("Toggle AI"))
        {
            cont.aiIsActive = !cont.aiIsActive;
        }

        if (Btn("Full Stop"))
        {
            cont.pastStates.Clear();
            cont.currentState = cont.stopState;
        }

        if (Btn("test"))
        {
            var decs = EditorUtils.FindAssetsByType<State>();
            var rand = UnityEngine.Random.Range(0, decs.Count);
            cont.currentState = decs[rand];
            Debug.Log(decs[rand]);
        }
    }

    private void PlayingButtons()
    {
        if (!Application.isPlaying) return;
    }

    private void NonPlayingButtons()
    {
        if (Application.isPlaying) return;
    }

    private bool Btn(String str)
    {
        return GUILayout.Button(str);
    }
}