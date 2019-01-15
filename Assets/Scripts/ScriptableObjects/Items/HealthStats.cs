using UnityEngine;

[CreateAssetMenu(fileName = "HealthStats", menuName = "Items/Health")]
public class HealthStats : ScriptableObject 
{
    [Header("Shield")]
    public float maxHealth = 100;
    public float minHealth = 0;
    public float startingHealth = 100;

    [Header("Shield")]
    public float maxShield = 100;
    public float minShield = 0;
    public float startingShield = 100;

    [Header("Armor")]
    public float maxArmor = 100;
    public float minArmor = 0;
    public float startingArmor = 0;
}