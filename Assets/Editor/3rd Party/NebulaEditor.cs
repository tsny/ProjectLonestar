using UnityEngine;
using UnityEditor;
using EGL = UnityEditor.EditorGUILayout;

[CustomEditor(typeof(Nebula))]
public class NebulaEditor : Editor
{
    Nebula neb;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        neb = target as Nebula;

        EGL.Space();
        EGL.FloatField("Radius", neb.dimensions.magnitude);
    }

    private void OnDrawGizmos()
    {
        if (neb != null)
        {
            Gizmos.DrawWireSphere(neb.transform.position, neb.dimensions.magnitude);
        }
    }
}