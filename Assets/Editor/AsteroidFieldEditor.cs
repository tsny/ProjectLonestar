using UnityEditor;
using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;

[CustomEditor(typeof(AsteroidField))]
public class AsteroidFieldEditor : Editor 
{
    public bool usingVariableScale;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AsteroidField field = (AsteroidField)target;

        field.AsteroidGameObject = (GameObject) EGL.ObjectField("Asteroid Game Object", field.AsteroidGameObject, typeof(GameObject), false);

        field.InnerRadius = EGL.FloatField("Inner Radius", field.InnerRadius);
        field.OuterRadius = EGL.Slider("Outer Radius", field.OuterRadius, field.InnerRadius, 10000 + field.InnerRadius);

        usingVariableScale = EGL.Toggle("Use range for scale?", usingVariableScale);

        if (usingVariableScale)
        {
            field.AsteroidScale = EGL.Slider("Scale", field.AsteroidScale, 0, 5);
        }

        else
        {
            field.AsteroidScale = EGL.FloatField("Scale", field.AsteroidScale);
        }

        using (new EditorGUI.DisabledScope(field.AsteroidGameObject == null))
        {
            if(GUILayout.Button("Generate Field"))
            {
                field.GenerateField();
            }
        }

        if(GUILayout.Button("Clear Field"))
        {
            field.ClearField();
        }
    }
}
