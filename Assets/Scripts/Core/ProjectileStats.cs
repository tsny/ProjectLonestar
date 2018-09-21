using UnityEngine;

[CreateAssetMenu]
public class ProjectileStats : ScriptableObject
{
    public new string name = "Projectile";

    public float hullDamage = 5;
    public float shieldDamage = 5;
    public float thrust = 20;
    public float range = 600;
    public float cooldownDuration = 1;
    public float energyDraw = 1;

    public DamageType damageType;
}


