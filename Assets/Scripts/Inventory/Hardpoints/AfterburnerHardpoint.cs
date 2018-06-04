using UnityEngine;

public class AfterburnerHardpoint : Hardpoint
{
    public float charge = 100;
    public Afterburner afterburner;
    public bool engaged;
    public float regenRate;
    public float thrust;
    public float drain;

    public AfterburnerHardpoint()
    {
        associatedEquipmentType = typeof(Afterburner);
    }

    private void Update()
    {
        if (engaged)
        {
            if (charge > 0) charge -= (drain * Time.deltaTime);

            else
            {
                Disable();
                OnCooldown = true;
            }

        }

        else
        {
            charge = Mathf.MoveTowards(charge, 100, regenRate * Time.deltaTime);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        hardpointSystem.afterburnerHardpoint = this;
    }

    public override void Mount(Equipment newEquipment)
    {
        base.Mount(newEquipment);

        afterburner = newEquipment as Afterburner;

        regenRate = afterburner.regenRate;
        thrust = afterburner.thrust;
        drain = afterburner.drain;
    }

    public void Enable()
    {
        if(!OnCooldown && afterburner != null)
        {
            engaged = true;
        }
    }

    public void Disable()
    {
        engaged = false;
    }
}
