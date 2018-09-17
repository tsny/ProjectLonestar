using System.Collections;
using UnityEngine;

public class ShieldHardpoint : Hardpoint
{
    private AudioSource hitSource;

    public float capacity;
    public float health;
    public float regenRate;

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
            return health > 0;
        }
    }

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
        health = capacity;
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
         health = Mathf.MoveTowards(health, capacity, regenRate * Time.deltaTime);
    }

    public void TakeDamage(Weapon weapon)
    {
        health -= Damage.CalculateShieldDamage(weapon, Shield.type);
        hitSource.Play();

        if (health <= 0) health = 0;

        StartCooldown();
    }
}
