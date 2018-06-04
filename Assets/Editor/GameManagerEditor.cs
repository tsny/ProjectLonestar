using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager manager = (GameManager)target;
    }
}
