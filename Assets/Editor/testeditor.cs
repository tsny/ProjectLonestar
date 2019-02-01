using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(test))]
public class testeditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        var thing = target as test;
        
        EditorGUI.BeginChangeCheck();

        var testee = (GameObject) EditorGUILayout.ObjectField("EditorTest", thing.tester, typeof(GameObject), false);

        if (EditorGUI.EndChangeCheck() && testee != null)
        {
            thing.method(testee);
        }

    }
}