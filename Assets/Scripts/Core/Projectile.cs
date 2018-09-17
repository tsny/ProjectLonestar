using UnityEngine;

[CreateAssetMenu]
public class Projectile : ScriptableObject
{
    public new string name;
    public float hullDamage;
    public float shieldDamage;
    public float thrust;
    public float range;
    public DamageType damageType;
}


