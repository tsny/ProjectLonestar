using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipNavigation))]
public class ShipNavigationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var shipNav = target as ShipNavigation;

        if (GUILayout.Button("Fire at player"))
        {
            shipNav.FireAtPlayer();
        }
    }
}