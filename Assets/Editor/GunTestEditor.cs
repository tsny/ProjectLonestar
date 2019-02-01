using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GunTest))]
public class GunTestEditor : Editor 
{
    GunTest gunTest;

    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
            
        gunTest = target as GunTest;        

        if (GUILayout.Button("Fire"))
        {
            gunTest.Fire();
        }
    }
}