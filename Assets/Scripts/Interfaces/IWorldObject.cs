using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldObject 
{
    void TakeDamage(Weapon weapon);
    void Die();
}

public interface IPossessable
{
    void Possessed(PlayerController sender);
    void UnPossessed(PlayerController sender);
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

