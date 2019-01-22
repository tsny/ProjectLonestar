using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Loot))]
public class LootEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        var loot = target as Loot;         

        EditorGUILayout.LabelField("Distance To Target: " + loot.DistanceToTarget);

        if (loot.baseSystem && loot.subSystem)
        {
            loot.SetParticleColors(loot.grad);
        }
        else
        {
            EditorGUILayout.HelpBox("Particle System(s) are null...", MessageType.Error);
        }
    }
}