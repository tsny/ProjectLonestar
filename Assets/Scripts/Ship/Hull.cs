using UnityEngine;
using System.Collections;

public class Hull : HealthComponent
{
    public float health = 100;
    public float maxHealth = 100;
    public bool invulnerable = false;
    public HullType hullType = HullType.Light;
    public ShieldHardpoint shieldHardpoint;

    public delegate void DamageEventHandler(MonoBehaviour sender, DamageEventArgs e);
    public delegate void DeathEventHandler(MonoBehaviour sender, DeathEventArgs e);

    public event DeathEventHandler HealthDepleted;
    public event DamageEventHandler TookDamage;

    public override void Setup(Ship sender)
    {
        base.Setup(sender);

        hullType = sender.stats.hullType;
        health = sender.stats.maxHealth;
        maxHealth = sender.stats.maxHealth;

        shieldHardpoint = sender.hardpointSystem.shieldHardpoint;
    }

    private void Awake()
    {
        health = maxHealth;
    }

    public override void OnHealthDepleted(Weapon weapon)
    {
        if (HealthDepleted != null) HealthDepleted(this, new DeathEventArgs(weapon, gameObject.transform.position));
    }

    public override void TakeDamage(Weapon weapon)
    {
        if (invulnerable) return;

        if (shieldHardpoint != null && shieldHardpoint.IsMounted && shieldHardpoint.IsOnline)
        {
            shieldHardpoint.TakeDamage(weapon);
            return;
        }

        health -= weapon.hullDamage;

        if (health <= 0)
        {
            if (TookDamage != null) TookDamage(this, new DamageEventArgs(weapon));
            OnHealthDepleted(weapon);
        }
    }
}
