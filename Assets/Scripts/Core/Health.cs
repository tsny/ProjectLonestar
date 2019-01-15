using UnityEngine;
using System;

public class Health : ScriptableObject
{
    public bool invulnerable;

    public float shield;
    public float maxShield = 100;
    public float minShield = 0;
    public float startingShield = 100;

    [Space(20)]
    public float armor;
    public float maxArmor = 100;
    public float minArmor = 0;
    public float startingArmor = 0;

    [Space(20)]
    public float health;
    public float maxHealth = 100;
    public float minHealth = 0;
    public float startingHealth = 100;

    public event EventHandler TookDamage;
    public event EventHandler HealthDepleted;

    public delegate void EventHandler();

    private void OnTookDamage(WeaponStats weapon)
    {
        if (TookDamage != null) TookDamage();
    }

    private void OnHealthDepleted()
    {
        if (HealthDepleted != null) HealthDepleted();
    }

    public virtual void TakeDamage(WeaponStats weapon)
    {
        if (invulnerable) return;

        if (shield >= 0)
        {
            shield -= weapon.shieldDamage;
        }

        else if (armor >= 0)
        {
            // Do some more calculations here
            armor -= weapon.hullDamage;
        }

        else if (health >= 0)
        {
            health -= weapon.hullDamage;
        }

        OnTookDamage(weapon);

        if (health <= 0) OnHealthDepleted();
    }
    
    public static Health Immortal
    {
        get
        {
            var inst = CreateInstance<Health>();
            inst.health = 99999;
            return inst;
        }
    }

    public void Init()
    {
        health = startingHealth;
        shield = startingShield;
        armor = startingArmor;
    } 
}