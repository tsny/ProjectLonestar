using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ship))]
public class ShipEditor : Editor
{
    ShipController playerController;
    Ship ship;

    private void Awake()
    {
        playerController = FindObjectOfType<ShipController>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ship = target as Ship;

        if (playerController.controlledShip != ship)
        {
            ShowInspectorPossessionControls();
        }

        else
        {
            ShowInspectorUnpossessionControls();
        }
    }

    void OnSceneGUI()
    {
        ship = target as Ship;

        if (playerController.controlledShip != ship)
        {
            ShowPossessionHandle();
        }
    }

    private void ShowPossessionHandle()
    {
        if (Handles.Button(ship.transform.position + Vector3.up * 10, Quaternion.identity, 3, 3, Handles.SphereHandleCap))
        {
            playerController.Possess(ship);
        }
    }

    private void ShowInspectorPossessionControls()
    {
        if (GUILayout.Button("Possess"))
        {
            playerController.Possess(ship);
        }
    }

    private void ShowInspectorUnpossessionControls()
    {
        if (GUILayout.Button("Unpossess"))
        {
            playerController.UnPossess();
        }
    }
}