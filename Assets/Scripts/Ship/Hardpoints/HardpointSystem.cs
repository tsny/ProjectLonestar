using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HardpointSystem : MonoBehaviour
{
    public List<WeaponHardpoint> weaponHardpoints = new List<WeaponHardpoint>();

    public Ship ship;

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

    private void Awake()
    {
        ship = GetComponentInParent<Ship>();
    }

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
        foreach (WeaponHardpoint hardpoint in weaponHardpoints)
        {
            if (hardpoint.Active)
            {
                FireWeaponHardpoint(hardpoint, target);
            }
        }
    }

    public void FireWeaponHardpoint(int hardpointSlot, Vector3 target)
    {
        hardpointSlot--;

        if (hardpointSlot >= weaponHardpoints.Capacity || hardpointSlot < 0) return;

        if (weaponHardpoints[hardpointSlot] != null)
        {
            FireWeaponHardpoint(weaponHardpoints[hardpointSlot], target);
        }
    }

    public void FireWeaponHardpoint(WeaponHardpoint hardpointToFire, Vector3 target)
    {
        if (CanFireWeapons == false) return;

        if (hardpointToFire.projectilePrefab.weaponStats.energyDraw < energy)
        {
            if (hardpointToFire.Fire(target, ship.GetComponentsInChildren<Collider>()))
            {
                energy -= hardpointToFire.projectilePrefab.weaponStats.energyDraw;
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
