using System.Linq;
using UnityEditor;
using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;

[CustomEditor(typeof(AsteroidField))]
public class AsteroidFieldEditor : Editor 
{
    AsteroidField field;

    public override void OnInspectorGUI()
    {
        field = (AsteroidField)target;

        EGL.LabelField("Settings", EditorStyles.boldLabel);
        field.hideFlags = (HideFlags) EGL.EnumPopup("Hide Flags", field.hideFlags);
        field.desiredAsteroids = EGL.IntField("Desired Asteroids", field.desiredAsteroids);
        EGL.Space();

        EGL.LabelField("GameObject", EditorStyles.boldLabel);
        ShowGameObjectInformation();
        EGL.Space();

        EGL.LabelField("Radius", EditorStyles.boldLabel);
        ShowRadiusInfromation();
        EGL.Space();

        EGL.LabelField("Scale", EditorStyles.boldLabel);
        ShowScaleInformation();
        EGL.Space();

        ShowButtons();
    }

    private void ShowScaleInformation()
    {
        field.scaleUsesRange = EGL.Toggle(new GUIContent("Scale uses range?", "Whether the asteroids spawned should pick a scale from a specified range."), field.scaleUsesRange);

        if (field.scaleUsesRange)
        {
            field.asteroidLowerScale = EGL.Slider("Lower range", field.asteroidLowerScale, 0, 20);
            field.asteroidUpperScale = EGL.Slider("Upper range", field.asteroidUpperScale, field.asteroidLowerScale, field.asteroidLowerScale + 20);
        }

        else
        {
            field.asteroidScaleMultiplier = EGL.Slider("Scale Multiplier", field.asteroidScaleMultiplier, 0, 20);
        }
    }

    private void ShowRadiusInfromation()
    {
        field.innerRadius = EGL.FloatField(new GUIContent("Inner Radius", "The inner radius of the field"), field.innerRadius);
        field.outerRadius = EGL.Slider("Outer Radius", field.outerRadius, field.innerRadius, 10000 + field.innerRadius);
    }

    private void ShowGameObjectInformation()
    {
        field.useArrayOfAsteroids = EGL.Toggle("Use Array", field.useArrayOfAsteroids);

        if (field.useArrayOfAsteroids)
        {
            SerializedObject serialObject = new SerializedObject(target);
            SerializedProperty serialProperty = serialObject.FindProperty("asteroids");

            EGL.PropertyField(serialProperty, new GUIContent("Asteroids"), true);

            serialObject.ApplyModifiedProperties();
        }

        else
        {
            var newAsteroidGameObject = (GameObject) EGL.ObjectField(new GUIContent("Asteroid Game Object", "The game object that represents the spawned asteroids"), field.asteroid, typeof(GameObject), false); 

            if (GUI.changed)
            {
                field.asteroid = newAsteroidGameObject;
            }
        }
    }

    private void ShowButtons()
    { 
        using (new EditorGUI.DisabledScope(field.asteroid == null))
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
