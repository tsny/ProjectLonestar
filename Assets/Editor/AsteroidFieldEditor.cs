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
        AsteroidField field = (AsteroidField)target;

        field.AsteroidGameObject = (GameObject) EGL.ObjectField("Asteroid Game Object", field.AsteroidGameObject, typeof(GameObject), false);

        field.InnerRadius = EGL.FloatField("Inner Radius", field.InnerRadius);
        field.OuterRadius = EGL.Slider("Outer Radius", field.OuterRadius, field.InnerRadius, 10000 + field.InnerRadius);

        scaleUsesRange = EGL.Toggle("Use range for scale?", scaleUsesRange);

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
