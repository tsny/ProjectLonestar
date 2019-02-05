using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor 
{
    PlayerController cont;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        cont = target as PlayerController;

        if (cont.CurrentAimPosition != null)
            EditorGUILayout.TextArea(cont.CurrentAimPosition.ToString());

        if (cont.ship != null)
            ShowInspectorUnpossessionControls();

        PossessNewButton();
    }

    private void ShowInspectorUnpossessionControls()
    {
        if (GUILayout.Button("Release")) cont.Release();
    }

    private void PossessNewButton()
    {
        if (Application.isPlaying && GUILayout.Button("Possess New Ship"))
        {
            var ship = Instantiate(GameSettings.Instance.defaultShip, cont.transform.position, Quaternion.identity);
            cont.Possess(ship);
        }
    }
}
