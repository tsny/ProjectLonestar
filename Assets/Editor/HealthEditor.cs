using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        var hp = target as Health;

        if (Application.isPlaying)
        {
            EditorGUI.BeginChangeCheck();
            var newHp = EditorGUILayout.Slider("Health", hp.Amount, 0, hp.stats.maxHealth);
            if (EditorGUI.EndChangeCheck())
            {
                hp.Amount = newHp;
            }

            EditorGUI.BeginChangeCheck();
            var newShield = EditorGUILayout.Slider("Shield", hp.Shield, 0, hp.stats.maxShield);
            if (EditorGUI.EndChangeCheck())
            {
                hp.Shield = newShield;
            }

            if (GUILayout.Button("Deplete"))
            {
                hp.Deplete();
            }

            if (GUILayout.Button("Regen To Full"))
            {
                hp.FullHealth();
            }
        }

        hp.Invulnerable = EditorGUILayout.Toggle("Invulnerable", hp.Invulnerable);
    }
}