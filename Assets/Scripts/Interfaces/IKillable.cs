using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    float Health { get; set; }
    float MaxHealth { get; set; }

    void TakeDamage(WeaponStats weapon);

    event EventHandler<DamageEventArgs> TookDamage;
    event EventHandler<DeathEventArgs> HealthDepleted;
}

public class DamageEventArgs : EventArgs
{
    public WeaponStats weapon;
    public bool shieldDamage;
    public float damage;

    public DamageEventArgs(bool shieldDamage, float damage)
    {
        this.shieldDamage = shieldDamage;
        this.damage = damage;
    }

    public DamageEventArgs(WeaponStats weapon)
    {
        this.weapon = weapon;
    }
}

public class DeathEventArgs : EventArgs
{
    public WeaponStats weaponUsed;
    public Vector3 deathLocation;
    public DateTime timeOfDeath;

    public DeathEventArgs(WeaponStats weaponUsed, Vector3 deathLocation)
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

