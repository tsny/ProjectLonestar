using UnityEngine;
using EGL = UnityEditor.EditorGUILayout;
using UnityEditor;

[CustomEditor(typeof(AfterburnerHardpoint))]
public class AfterburnerHardpointEditor : Editor
{
    AfterburnerHardpoint afterburner;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        afterburner = target as AfterburnerHardpoint;

        EGL.LabelField("Is Active?", afterburner.IsActive.ToString());
        EGL.LabelField("Is On Cooldown?", afterburner.IsOnCooldown.ToString());

        ShowButtons();
    }

    private void ShowButtons()
    {
        if (Application.isPlaying == false) return;

        var buttonString = afterburner.IsActive ? "Deactivate" : "Activate";

        if (GUILayout.Button(buttonString))
        {
            if (afterburner.IsActive)
            {
                afterburner.Deactivate();
            }

            else
            {
                afterburner.Activate();
            }
        }
    }
}