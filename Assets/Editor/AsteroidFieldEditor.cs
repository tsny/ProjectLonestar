using UnityEditor;
using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;

[CustomEditor(typeof(AsteroidField))]
public class AsteroidFieldEditor : Editor 
{
    public bool scaleUsesRange;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EGL.Space();

        AsteroidField field = (AsteroidField)target;

        field.AsteroidGameObject = (GameObject) EGL.ObjectField(new GUIContent("Asteroid Game Object", "The game object that represents the spawned asteroids"), field.AsteroidGameObject, typeof(GameObject), false);

        field.InnerRadius = EGL.FloatField(new GUIContent("Inner Radius", "The inner radius of the field"), field.InnerRadius);
        field.OuterRadius = EGL.Slider("Outer Radius", field.OuterRadius, field.InnerRadius, 10000 + field.InnerRadius);

        EGL.Space();

        scaleUsesRange = EGL.Toggle(new GUIContent("Scale uses range?", "Whether the asteroids spawned should pick a scale from a specified range."), scaleUsesRange);

        if (scaleUsesRange)
        {
            field.AsteroidLowerScale = EGL.Slider("Lower range", field.AsteroidLowerScale, 0, 20);
            field.AsteroidUpperScale = EGL.Slider("Upper range", field.AsteroidUpperScale, field.AsteroidLowerScale, field.AsteroidLowerScale + 20);
        }

        else
        {
            field.AsteroidScale = EGL.Slider("Scale", field.AsteroidScale, 0, 20);
        }

        using (new EditorGUI.DisabledScope(field.AsteroidGameObject == null))
        {
            if(GUILayout.Button("Generate Field"))
            {
                field.GenerateField(scaleUsesRange);
            }
        }

        if(GUILayout.Button("Clear Field"))
        {
            field.ClearField();
        }
    }
}
