using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TractorHardpoint))]
public class TractorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        TractorHardpoint tractor = (TractorHardpoint)target;
        base.OnInspectorGUI();

        if(GUILayout.Button("LootAll"))
        {
            tractor.TractorAllLoot();
        }
    }
}
