using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AsteroidField))]
public class AsteroidFieldEditor : Editor 
{
    private float lowerScale = 1;
    private float upperScale = 3;

    private float asteroidScale = 1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AsteroidField field = (AsteroidField)target;

        if (field.scaleUsesRange)
        {
            field.scaleUsesRange = true;

            EditorGUILayout.BeginHorizontal();

            lowerScale = EditorGUILayout.FloatField(lowerScale);
            upperScale = EditorGUILayout.FloatField(upperScale);

            EditorGUILayout.EndHorizontal();

            if(GUILayout.Button("Generate Field"))
            {
                field.GenerateField(new AsteroidField.ScaleInfo(lowerScale, upperScale));
            }
        }

        else
        {
            asteroidScale = EditorGUILayout.FloatField("Scale", asteroidScale);

            if(GUILayout.Button("Generate Field"))
            {
                field.GenerateField(new AsteroidField.ScaleInfo(asteroidScale));
            }
        }

        if(GUILayout.Button("Clear Field"))
        {
            field.ClearField();
        }
    }
}
