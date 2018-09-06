using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ship))]
public class ShipEditor : Editor
{
    PlayerController playerController;
    Ship ship;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ship = target as Ship;
        DecidePossessionButtonVisibility();
    }

    void OnSceneGUI()
    {
        ship = target as Ship;

        if (!Application.isPlaying) return;

        if (playerController.controlledShip != ship) ShowPossessionHandle();
    }

    private void ShowPossessionHandle()
    {
        if (Handles.Button(ship.transform.position + Vector3.up * 10, Quaternion.identity, 3, 3, Handles.SphereHandleCap))
        {
            playerController.Possess(ship);
        }
    }

    private void DecidePossessionButtonVisibility()
    {
        if (!Application.isPlaying) return;

        if (playerController.controlledShip != ship)
        {
            if (GUILayout.Button("Possess"))
            {
                playerController.Possess(ship);
            }
        }

        else
        {
            if (GUILayout.Button("Unpossess"))
            {
                playerController.UnPossess();
            }
        }
    }
}