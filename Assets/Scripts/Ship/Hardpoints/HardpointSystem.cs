using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HardpointSystem : ShipComponent
{
    public List<Gun> guns = new List<Gun>();

    [Header("Individual Hardpoints")]

    public Afterburner afterburnerHardpoint;
    public Shield shieldHardpoint;
    public Scanner scannerHardpoint;
    public TractorBeam tractorHardpoint;
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

    public delegate void WeaponFiredEventHandler(Gun gunFired);
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

    public void FireActiveWeapons(Vector3 target)
    {
        foreach (Gun guns in guns)
        {
            if (guns.Active)
            {
                FireWeaponHardpoint(guns, target);
            }
        }
    }

    public void FireWeaponHardpoint(int hardpointSlot, Vector3 target)
    {
        hardpointSlot--;

        if (hardpointSlot >= guns.Capacity || hardpointSlot < 0) return;

        if (guns[hardpointSlot] != null)
        {
            FireWeaponHardpoint(guns[hardpointSlot], target);
        }
    }

    public void FireWeaponHardpoint(Gun gunToFire, Vector3 target)
    {
        if (CanFireWeapons == false) return;

        if (gunToFire.projectilePrefab.weaponStats.energyDraw < energy)
        {
            if (gunToFire.Fire(target, owningShip.GetComponentsInChildren<Collider>()))
            {
                energy -= gunToFire.projectilePrefab.weaponStats.energyDraw;
                if (WeaponFired != null) WeaponFired(gunToFire);
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
