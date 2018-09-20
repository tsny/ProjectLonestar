using System;
using System.Collections;
using UnityEngine;

public class ShieldHardpoint : Hardpoint, IDamageable
{
    private AudioSource hitSource;

    public float capacity;
    public float regenRate;

    public event EventHandler<DamageEventArgs> TookDamage;
    public event EventHandler<DeathEventArgs> HealthDepleted;

    public Shield Shield
    {
        get
        {
            return CurrentEquipment as Shield;
        }
    }

    public bool IsOnline
    {
        get
        {
            return Health > 0;
        }
    }

    public float Health { get; set; }
    public float MaxHealth { get; set; }

    protected override bool EquipmentMatchesHardpoint(Equipment equipment)
    {
        return equipment is Shield;
    }

    private void Awake()
    {
        hitSource = GetComponent<AudioSource>();
    }

    protected override void OnMounted(Equipment newEquipment)
    {
        base.OnMounted(newEquipment);

        capacity = Shield.capacity;
        Health = capacity;
        regenRate = Shield.regenRate;
    }

    private void Update()
    {
        if (!OnCooldown)
        {
            Recharge();
        }
    }

    private void Recharge()
    {
         Health = Mathf.MoveTowards(Health, capacity, regenRate * Time.deltaTime);
    }

    public void TakeDamage(Weapon weapon)
    {
        Health -= Damage.CalculateShieldDamage(weapon, Shield.type);

        hitSource.Play();

        if (Health <= 0) Health = 0;

        StartCooldown();
    }

    public void OnHealthDepleted(Weapon weapon)
    {
        if (HealthDepleted != null) HealthDepleted(this, null);
    }
}
