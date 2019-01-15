using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public bool invulnerable;
    public HealthStats stats;

    public float health = 100;
    public float shield = 100;
    public float armor;

    public event EventHandler TookDamage;
    public event EventHandler HealthDepleted;

    public delegate void EventHandler();

    private void Awake() 
    {
        Init();
    }

    public void Init()
    {
        if (stats == null)
        {
            stats = HealthStats.CreateInstance<HealthStats>();
        }

        health = stats.startingHealth;
        shield = stats.startingShield;
        armor = stats.startingArmor;
    } 

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

        if (shield >= stats.minShield)
        {
            shield -= weapon.shieldDamage;
        }

        else if (armor >= stats.minArmor)
        {
            // Do some more calculations here
            armor -= weapon.hullDamage;
        }

        else if (health >= stats.minHealth)
        {
            health -= weapon.hullDamage;
        }

        OnTookDamage(weapon);

        if (health <= stats.minHealth) OnHealthDepleted();
    }
}