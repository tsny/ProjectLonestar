using UnityEngine;

public class ShieldHardpoint : Hardpoint, IDamageable
{
    private AudioSource hitSource;

    public float capacity;
    public float health;
    public float regenRate;
    
    private Timer damagedTimer;
    private Shield shield;

    public bool IsOnline
    {
        get
        {
            return (OnCooldown == false && IsMounted == true);
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
        capacity = health;
    }

    protected override void Awake()
    {
        base.Awake();

        hardpointSystem.shieldHardpoint = this;
        hitSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!OnCooldown && damagedTimer == null) health = Mathf.MoveTowards(health, capacity, regenRate * Time.deltaTime);
    }

    public void TakeDamage(Weapon weapon)
    {
        if (shield == null) return;

        float calculatedDamage = Damage.CalculateShieldDamage(weapon, shield.type);
        health -= calculatedDamage;

        hitSource.Play();

        if (damagedTimer == null)
        {
            damagedTimer = gameObject.AddComponent<Timer>();
            damagedTimer.Initialize(2f);
        }

        else damagedTimer.Restart(true);

        if(health <= 0)
        {
            health = 0;
            BeginCooldown();
        }
    }
}
