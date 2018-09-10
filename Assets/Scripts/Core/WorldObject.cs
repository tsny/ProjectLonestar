using System;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public bool invulnerable;
    public float hullHealth = 100;
    public float hullFullHealth = 100;

    public delegate void WorldObjectKilledEventHandler(WorldObject sender, DeathEventArgs e);
    public delegate void WorldObjectDamagedEventHandler(WorldObject sender, DamageEventArgs e);

    public event WorldObjectKilledEventHandler Killed;
    public event WorldObjectDamagedEventHandler TookDamage;

    protected virtual void OnKilled()
    {
        if (Killed != null) Killed(this, new DeathEventArgs());
    }

    protected virtual void GenerateName()
    {
        name = "World Object";
    }

    protected virtual void Awake()
    {
        GenerateName();
    }

    public virtual void OnTookDamage(bool tookShieldDamage, float damage)
    {
        var damageEvent = TookDamage;

        if (damageEvent != null)
        {
            damageEvent(this, new DamageEventArgs(tookShieldDamage, damage));
        }
    }

    public virtual void TakeDamage(Weapon weapon)
    {
        if (invulnerable)
        {
            return;
        }
    }

    protected virtual void Die()
    {
        // Play FX
        OnKilled();
        Destroy(gameObject);
    }
}