using UnityEngine;
using System;

public class Hull : ShipComponent, IDamageable
{
    public bool invulnerable = false;
    public HullType hullType = HullType.Light;

    public event EventHandler<DamageEventArgs> TookDamage;
    public event EventHandler<DeathEventArgs> HealthDepleted;

    [SerializeField] private float _health = 100;
    [SerializeField] private float _maxHealth = 100;

    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    public Shield shield;

    protected void OnHealthDepleted(WeaponStats weapon)
    {
        if (HealthDepleted != null) HealthDepleted(this, new DeathEventArgs(weapon, gameObject.transform.position));
    }

    protected void OnTookDamage(WeaponStats weapon)
    {
        if (TookDamage != null) TookDamage(this, new DamageEventArgs(weapon));
    }

    public void TakeDamage(WeaponStats weapon)
    {
        if (invulnerable) return;

        if (shield != null)
        {
            if (shield.IsOnline)
            {
                shield.TakeDamage(weapon);
                return;
            }

            shield.StartCooldown(weapon.cooldownDuration);
        }

        Health -= weapon.hullDamage;

        if (Health <= 0)
        {
            OnHealthDepleted(weapon);
        }

        OnTookDamage(weapon);
    }
}
