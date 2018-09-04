using System;
using UnityEngine;

public enum WorldObjectType
{
    Ship,
    Station,
    Loot,
    Wreck,
    Jumpgate,
    Tradelane,
    Star,
    Anomaly,
    Jumphole,
    AsteroidField
}

public class WorldObject : MonoBehaviour
{
    protected WorldObjectType worldObjectType;

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

    protected virtual void SetName()
    {
        name = "World Object";
    }

    protected virtual void Awake()
    {
        SetName();
    }

    public virtual void SetupTargetIndicator(TargetIndicator indicator) { }

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

public class DamageEventArgs : EventArgs
{
    public bool shieldDamage;
    public float damage;

    public DamageEventArgs(bool shieldDamage, float damage)
    {
        this.shieldDamage = shieldDamage;
        this.damage = damage;
    }
}

public class DeathEventArgs : EventArgs
{
    public Weapon weaponUsed;
    public Vector3 deathLocation;
    public DateTime timeOfDeath;

    public DeathEventArgs(Weapon weaponUsed, Vector3 deathLocation)
    {
        this.weaponUsed = weaponUsed;
        this.deathLocation = deathLocation;
        timeOfDeath = DateTime.Now;
    }

    public DeathEventArgs()
    {
        this.weaponUsed = null;
        this.deathLocation = Vector3.zero;
        timeOfDeath = DateTime.Now;
    }
}









