using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HUDManager))]
public class HUDManagerEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
            
        var hud = target as HUDManager;

        if (Application.isPlaying && GUILayout.Button("Spawn Noti"))
        {
            hud.SpawnNotification("hello");
        }
    }
}