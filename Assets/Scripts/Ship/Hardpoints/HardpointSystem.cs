using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HardpointSystem : ShipComponent
{
    public List<Gun> guns = new List<Gun>();

    [Header("Individual Hardpoints")]

    public Afterburner afterburner;
    public Shield shield;
    public Scanner scanner;
    public TractorBeam tractorBeam;
    public CruiseEngine cruiseEngine;

    public Hardpoint[] hardpoints
    {
        get
        {
            return GetComponentsInChildren<Hardpoint>();
        }
    }

    [Header("Energy")]

    public float energy = 100;
    public float energyCapacity = 100;
    public float chargeRate = .2f;
    public float timeTillRecharge = 3;

    private float avgWepRange;
    public float AverageWeaponRange
    {
        get
        {
            if (avgWepRange != 0)
            {
                float totalRange = 0;
                foreach (var gun in guns) totalRange += gun.stats.range;
                avgWepRange = totalRange / guns.Count;
            } 
            
            return avgWepRange;
        }
    }

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
        if (toggle) afterburner.Activate();
        else afterburner.Deactivate();
    }

    public void FireActiveWeapons(Vector3 target)
    {
        foreach (Gun gun in guns)
            if (gun.IsActive) FireWeaponHardpoint(gun, target);
    }

    public void FireActiveWeapons(Rigidbody target)
    {
        foreach (Gun gun in guns)
            if (gun.IsActive) FireWeaponHardpoint(gun, target);
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

    public void FireWeaponHardpoint(Gun gun, Vector3 target)
    {
        if (CanFireWeapons == false || gun.stats == null) return;

        if (gun.stats.energyDraw < energy && gun.Fire(target, ship.colliders))
        {
            energy -= gun.stats.energyDraw;
            if (WeaponFired != null) WeaponFired(gun);
            BeginCooldown();
        }
    }

    public void FireWeaponHardpoint(Gun gun, Rigidbody target)
    {
        if (CanFireWeapons == false || gun.stats == null) return;

        if (gun.stats.energyDraw < energy && gun.FireAtMovingTarget(target, ship.colliders))
        {
            energy -= gun.stats.energyDraw;
            if (WeaponFired != null) WeaponFired(gun);
            BeginCooldown();
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
