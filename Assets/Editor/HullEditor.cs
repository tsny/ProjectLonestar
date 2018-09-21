using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;
using UnityEditor;

[CustomEditor(typeof(Hull))]
public class HullEditor : Editor
{
    private SerializedProperty m_Health;

    //public void OnEnable()
    //{
    //    m_Health = serializedObject.FindProperty("Health");
    //}

    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //    EGL.PropertyField(m_Health);

    //    serializedObject.ApplyModifiedProperties();

    //    base.OnInspectorGUI();
    //}
}