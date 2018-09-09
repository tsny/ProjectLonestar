using System.Collections;
using UnityEngine;

public class ShieldHardpoint : Hardpoint, IDamageable
{
    private AudioSource hitSource;

    public float capacity;
    public float health;
    public float regenRate;

    private Shield shield;

    public bool IsOnline
    {
        get
        {
            return health > 0;
        }
    }

    public ShieldHardpoint()
    {
        associatedEquipmentType = typeof(Shield);
    }

    public override void Mount(Equipment newEquipment)
    {
        base.Mount(newEquipment);

        shield = newEquipment as Shield;

        health = shield.capacity;
        capacity = shield.capacity;
        regenRate = shield.regenRate;
    }

    public override void Demount()
    {
        base.Demount();

        shield = null;

        health = 0;
        capacity = 0;
        regenRate = 0;
    }

    protected override void Awake()
    {
        base.Awake();

        hardpointSystem.shieldHardpoint = this;
        hitSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!OnCooldown)
        {
            Recharge();
        }
    }

    private void Recharge()
    {
         health = Mathf.MoveTowards(health, capacity, regenRate * Time.deltaTime);
    }

    public void TakeDamage(Weapon weapon)
    {
        health -= Damage.CalculateShieldDamage(weapon, shield.type);
        hitSource.Play();

        if (health <= 0)
        {
            health = 0;
        }

        StartCooldown();
    }
}
