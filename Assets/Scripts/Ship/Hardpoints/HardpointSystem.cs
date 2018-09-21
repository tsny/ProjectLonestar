using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HardpointSystem : ShipComponent
{
    public List<WeaponHardpoint> weaponHardpoints = new List<WeaponHardpoint>();

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
                case CruiseState.Off:
                case CruiseState.Disrupted:
                    return true;

                case CruiseState.Charging:
                case CruiseState.On:
                    return false;

                default:
                    return false;
            }
        }
    }

    public delegate void WeaponFiredEventHandler(WeaponHardpoint hardpointFired);
    public event WeaponFiredEventHandler WeaponFired;

    public void ToggleAfterburner(bool toggle)
    {
        if (toggle) afterburnerHardpoint.Activate();
        else afterburnerHardpoint.Deactivate();
    }

    public void EnableInfiniteEnergy()
    {
        energyCapacity = 1000000;
        energy = 1000000;
        chargeRate = 1000;
    }

    public void FireActiveWeapons()
    {
        foreach (WeaponHardpoint hardpoint in weaponHardpoints)
        {
            if (hardpoint.Active)
            {
                FireWeaponHardpoint(hardpoint);
            }
        }
    }

    public void FireWeaponHardpoint(int hardpointSlot)
    {
        if (weaponHardpoints[hardpointSlot] != null)
        {
            FireWeaponHardpoint(weaponHardpoints[hardpointSlot]);
        }
    }

    public void FireWeaponHardpoint(WeaponHardpoint hardpointToFire)
    {
        if (CanFireWeapons == false) return;

        if (hardpointToFire.projectilePrefab.projectileStats.energyDraw < energy)
        {
            if (hardpointToFire.Fire())
            {
                energy -= hardpointToFire.projectilePrefab.projectileStats.energyDraw;
                if (WeaponFired != null) WeaponFired(hardpointToFire);
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
