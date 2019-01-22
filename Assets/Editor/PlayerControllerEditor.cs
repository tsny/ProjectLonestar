using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor 
{
    PlayerController cont;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        cont = target as PlayerController;

        if (cont.CurrentAimPosition != null)
        {
            EditorGUILayout.TextArea(cont.CurrentAimPosition.ToString());
        }

        if (cont.ship != null)
        {
            ShowInspectorUnpossessionControls();
        }
    }

    private void ShowInspectorUnpossessionControls()
    {
        if (GUILayout.Button("Release"))
        {
            cont.Release();
        }
    }
}
