using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AsteroidField))]
public class AsteroidFieldEditor : Editor 
{
    public bool usingVariableScale;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AsteroidField field = (AsteroidField)target;

        usingVariableScale = EditorGUILayout.Toggle("Use range for scale?", usingVariableScale);

        if (usingVariableScale)
        {
            field.AsteroidScale = EditorGUILayout.Slider("Scale", field.AsteroidScale, 0, 5);
        }

        else
        {
            field.AsteroidScale = EditorGUILayout.FloatField("Scale", field.AsteroidScale);
        }

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
