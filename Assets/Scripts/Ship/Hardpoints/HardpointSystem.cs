using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HardpointSystem : ShipComponent
{
    public List<Hardpoint> hardpoints = new List<Hardpoint>();
    public Dictionary<int, WeaponHardpoint> weaponHardpoints = new Dictionary<int, WeaponHardpoint>();

    [Header("Individual Hardpoints")]

    public AfterburnerHardpoint afterburnerHardpoint;
    public ShieldHardpoint shieldHardpoint;
    public ScannerHardpoint scannerHardpoint;
    public TractorHardpoint tractorHardpoint;
    public CruiseEngine cruiseEngine;

    [Header("Energy")]

    public float energy = 100;
    public float energyCapacity = 100;
    public float chargeRate = .2f;
    public float timeTillRecharge = 3;

    private IEnumerator rechargeEnumerator;
    private IEnumerator cooldownEnumerator;

    public bool CanFireWeapons
    {
        get
        {
            if (cruiseEngine == null) return true;

            switch (cruiseEngine.State)
            {
                case CruiseEngine.CruiseState.Off:
                case CruiseEngine.CruiseState.Disrupted:
                    return true;

                case CruiseEngine.CruiseState.Charging:
                case CruiseEngine.CruiseState.On:
                    return false;

                default:
                    return false;
            }
        }
    }

    public delegate void WeaponFiredEventHandler(Weapon weapon);
    public event WeaponFiredEventHandler WeaponFired;

    private void Awake()
    {
        GetComponentsInChildren(true, hardpoints);

        DesignateWeaponSlots();

        foreach(Hardpoint hardpoint in hardpoints)
        {
            hardpoint.Mounted += HandleHardpointMounted;
            hardpoint.Demounted += HandleHardpointDemounted;
        }
    }

    public void SetStats(ShipStats stats)
    {
        energyCapacity = stats.energyCapacity;
        chargeRate = stats.energyChargeRate;
    }

    public override void InitShipComponent(Ship sender, ShipStats stats)
    {
        base.InitShipComponent(sender, stats);
        cruiseEngine = sender.cruiseEngine;
    }

    public void DemountAll()
    {
        foreach (Hardpoint hardpoint in hardpoints)
        {
            hardpoint.Demount();
        }
    }

    public void ToggleAfterburner(bool toggle)
    {
        if (toggle) afterburnerHardpoint.Activate();
        else afterburnerHardpoint.Disable();
    }

    public void EnableInfiniteEnergy()
    {
        energyCapacity = 1000000;
        energy = 1000000;
        chargeRate = 1000;
    }

    public void FireActiveWeapons()
    {
        foreach (WeaponHardpoint hardpoint in weaponHardpoints.Values)
        {
            if (hardpoint.active)
            {
                FireWeaponHardpoint(hardpoint);
            }
        }
    }

    public void FireWeaponHardpoint(int hardpointSlot)
    {
        WeaponHardpoint hardpointToFire;

        if (weaponHardpoints.TryGetValue(hardpointSlot, out hardpointToFire))
        {
            FireWeaponHardpoint(hardpointToFire);
        }
    }

    public void FireWeaponHardpoint(WeaponHardpoint hardpointToFire)
    {
        if (CanFireWeapons == false) return;

        if (hardpointToFire.Weapon.energyDraw < energy)
        {
            if (hardpointToFire.Fire())
            {
                energy -= hardpointToFire.Weapon.energyDraw;
                if (WeaponFired != null) WeaponFired(hardpointToFire.Weapon);
                BeginCooldown();
            }
        }
    }

    public void BeginCooldown()
    {
        if (cooldownEnumerator != null)
        {
            StopCoroutine(cooldownEnumerator);
        }

        cooldownEnumerator = CooldownCoroutine();
        StartCoroutine(cooldownEnumerator);
    }

    public void StopCooldown()
    {
        if (cooldownEnumerator == null) return;

        StopCoroutine(cooldownEnumerator);
        cooldownEnumerator = null; 
    }

    public void StartRecharging()
    {
        rechargeEnumerator = RechargeCoroutine();
        StartCoroutine(rechargeEnumerator);
    }

    public void StopRecharging()
    {
        if (rechargeEnumerator != null)
        {
            StopCoroutine(rechargeEnumerator);
        }

        rechargeEnumerator = null;
    }

    public void MountLoadout(Loadout newLoadout)
    {
        if (newLoadout == null) return;

        DemountAll();

        foreach (Equipment equipment in newLoadout.equipment)
        {
            foreach (Hardpoint hardpoint in hardpoints)
            {
                if (hardpoint.IsMounted) continue;

                if (hardpoint.TryMount(equipment))
                {
                    break;
                }
            }
        }
    }

    private void HandleHardpointDemounted(Hardpoint sender, Equipment oldEquipment)
    {
        WeaponHardpoint weaponHardpoint = sender as WeaponHardpoint;

        if (weaponHardpoint == null) return;

        weaponHardpoints.Remove(weaponHardpoint.slot);
    }

    private void HandleHardpointMounted(Hardpoint sender, Equipment newEquipment)
    {
        WeaponHardpoint weaponHardpoint = sender as WeaponHardpoint;

        if (weaponHardpoint == null) return;

        weaponHardpoints.Add(weaponHardpoint.slot, weaponHardpoint);
    }

    private void DesignateWeaponSlots()
    {
        int slotCounter = 1;

        foreach (WeaponHardpoint weaponHardpoint in hardpoints.Where(x => x.GetType() == typeof(WeaponHardpoint)))
        {
            weaponHardpoint.slot = slotCounter;
            slotCounter++;
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        StopRecharging();

        yield return new WaitForSeconds(timeTillRecharge);

        StartRecharging();
    }

    private IEnumerator RechargeCoroutine()
    {
        while (energy < energyCapacity)
        {
            energy += chargeRate;
            energy = Mathf.Clamp(energy, 0, energyCapacity);
            yield return null;
        }
    }
}
