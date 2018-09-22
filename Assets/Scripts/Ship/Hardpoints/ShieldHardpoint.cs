using System;
using System.Collections;
using UnityEngine;

public class ShieldHardpoint : Hardpoint
{
    private AudioSource hitSource;

    public float energy;
    public float capacity;
    public float regenRate;

    public event EventHandler<DamageEventArgs> TookDamage;
    public event EventHandler<DeathEventArgs> HealthDepleted;

    public ShieldStats shield;

    public bool IsOnline
    {
        get
        {
            return energy > 0;
        }
    }

     private void Awake()
     {
        hitSource = GetComponent<AudioSource>();
     }

    private void Update()
    {
        if (!IsOnCooldown)
        {
            Recharge();
        }
    }

    private void Recharge()
    {
         energy = Mathf.MoveTowards(energy, capacity, regenRate * Time.deltaTime);
    }

    public void TakeDamage(WeaponStats weapon)
    {
        energy -= Damage.CalculateShieldDamage(weapon, shield.type);

        hitSource.Play();

        if (energy <= 0) energy = 0;

        StartCooldown(shield.cooldownDuration);
    }

    public void OnHealthDepleted(WeaponStats weapon)
    {
        if (HealthDepleted != null) HealthDepleted(this, null);
    }
}
