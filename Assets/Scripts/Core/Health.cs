using UnityEngine;
using System;

public class Health : Component, IDamageable
{
    public float health;
    public float maxHealth = 100;
    public float minHealth = 0;

    public event EventHandler<DamageEventArgs> TookDamage;
    public event EventHandler<DeathEventArgs> HealthDepleted;

    public void TakeDamage(WeaponStats weapon)
    {
        throw new NotImplementedException();
    }
}