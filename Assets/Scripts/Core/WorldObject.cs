using System;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public bool invulnerable;
    public float hullHealth = 100;
    public float hullFullHealth = 100;

    public delegate void DeathEventHandler(WorldObject sender, DeathEventArgs e);
    public delegate void DamagedEventHandler(WorldObject sender, DamageEventArgs e);

    public event DeathEventHandler Killed;
    public event DamagedEventHandler TookDamage;

    protected virtual void OnKilled()
    {
        if (Killed != null) Killed(this, new DeathEventArgs());
    }

    public virtual string ToStringForScannerEntry()
    {
        return "World Object Scanner String";
    }

    public virtual string ToStringForIndicatorHeader()
    {
        return "World Object Indicator Header";
    }

    protected virtual void SetHierarchyName()
    {
        name = "World Object";
    }

    protected virtual void Awake()
    {
        SetHierarchyName();
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