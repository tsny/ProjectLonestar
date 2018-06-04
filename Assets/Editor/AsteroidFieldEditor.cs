using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AsteroidField))]
public class AsteroidFieldEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AsteroidField field = (AsteroidField)target;
        if(GUILayout.Button("Generate Field"))
        {
            field.GenerateField();
        }

        if(GUILayout.Button("Clear Field"))
        {
            field.ClearField();
        }
    }
}
