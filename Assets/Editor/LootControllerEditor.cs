using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Loot))]
public class LootControllerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        Loot controller = (Loot)target;
        base.OnInspectorGUI();

    }
}
