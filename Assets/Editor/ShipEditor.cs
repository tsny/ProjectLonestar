using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ship))]
public class ShipEditor : Editor
{
    void OnSceneGUI()
    {
        Ship ship = target as Ship;
        PlayerController playerController = FindObjectOfType<PlayerController>();

        if (playerController.controlledShip == ship) return;

        if (Handles.Button(ship.transform.position + Vector3.up * 10, Quaternion.identity, 3, 3, Handles.SphereHandleCap))
        {
            if (playerController == null)
            {
                playerController = new GameObject().AddComponent<PlayerController>();
            }

            playerController.Possess(ship);
        }
    }
}