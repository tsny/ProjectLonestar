using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor 
{
    PlayerController playerController;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        playerController = target as PlayerController;

        if (playerController.ship != null)
        {
            ShowInspectorUnpossessionControls();
        }
    }

    private void ShowInspectorUnpossessionControls()
    {
        if (GUILayout.Button("Release"))
        {
            playerController.Release();
        }
    }
}
