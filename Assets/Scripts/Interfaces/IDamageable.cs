using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    void TakeDamage(WeaponStats weapon);
}

public class DamageEventArgs : EventArgs
{
    public WeaponStats weapon;

    public bool damageWasAppliedToShield;
    public float damage;

    public DamageEventArgs(bool damageWasAppliedToShield, float damage)
    {
        this.damageWasAppliedToShield = damageWasAppliedToShield;
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

