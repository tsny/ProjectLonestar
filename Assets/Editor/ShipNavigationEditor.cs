using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShipNavigation))]
public class ShipNavigationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var shipNav = target as ShipNavigation;

        GUILayout.Space(20);

        if (Application.isPlaying && GUILayout.Button("Clear Target"))
        {
            shipNav.ClearTarget();
        }

        if (Application.isPlaying && GUILayout.Button("Random Target"))
        {
            var targets = FindObjectsOfType<MeshRenderer>();

            shipNav.target = targets[Random.Range(0, targets.Length)].gameObject.transform;

            shipNav.StopAllCoroutines();
        }

        if (Application.isPlaying && GUILayout.Button("Target Player"))
        {
            shipNav.TargetPlayer();
        }

        if (Application.isPlaying && GUILayout.Button("Fire at Target"))
        {
            shipNav.StartCoroutine(shipNav.fireRoutine, shipNav.FireAtTarget);
        }

        if (Application.isPlaying && GUILayout.Button("Goto Target"))
        {
            shipNav.StartCoroutine(shipNav.gotoRoutine, shipNav.GotoTarget);
        }

        if (Application.isPlaying && GUILayout.Button("Rotate Towards Target"))
        {
            shipNav.StartCoroutine(shipNav.rotateRoutine, shipNav.RotateTowardsTarget);
        }

        if (Application.isPlaying && GUILayout.Button("Attack Run"))
        {
            shipNav.StartCoroutine(shipNav.attackRoutine, shipNav.AttackRun);
        }

        if (Application.isPlaying && GUILayout.Button("Evade"))
        {
            shipNav.StartCoroutine(shipNav.evadeRoutine, shipNav.PitchEvade);
        }

        GUILayout.Space(20);

        if (Application.isPlaying && GUILayout.Button("Full Stop"))
        {
            shipNav.FullStop();
        }
    }
}