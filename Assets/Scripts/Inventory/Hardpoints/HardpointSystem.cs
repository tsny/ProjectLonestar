using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HardpointSystem : ShipComponent
{
    public List<Hardpoint> hardpoints = new List<Hardpoint>();
    public Dictionary<int, WeaponHardpoint> weaponHardpoints = new Dictionary<int, WeaponHardpoint>();

    [Header("Individual Hardpoints")]

    public AfterburnerHardpoint afterburnerHardpoint;
    public ShieldHardpoint shieldHardpoint;
    public ScannerHardpoint scannerHardpoint;
    public TractorHardpoint tractorHardpoint;

    [Header("Energy")]

    public float energy = 100;
    public float energyCapacity = 100;
    public float chargeRate = 3;

    protected override void Awake()
    {
        base.Awake();
        GetComponentsInChildren(true, hardpoints);

        DesignateWeaponSlots();

        foreach(Hardpoint hardpoint in hardpoints)
        {
            hardpoint.Mounted += HandleHardpointMounted;
            hardpoint.Demounted += HandleHardpointDemounted;
        }
    }

    private void HandleHardpointDemounted(Hardpoint sender)
    {
        WeaponHardpoint weaponHardpoint = sender as WeaponHardpoint;

        if (weaponHardpoint == null) return;

        weaponHardpoints.Remove(weaponHardpoint.slot);
    }

    private void HandleHardpointMounted(Hardpoint sender)
    {
        WeaponHardpoint weaponHardpoint = sender as WeaponHardpoint;

        if (weaponHardpoint == null) return;

        weaponHardpoints.Add(weaponHardpoint.slot, weaponHardpoint);
    }

    public void DemountAll()
    {
        foreach (Hardpoint hardpoint in hardpoints)
        {
            hardpoint.Demount();
        }
    }

    private void DesignateWeaponSlots()
    {
        int slotCounter = 1;

        foreach (Hardpoint hardpoint in hardpoints)
        {
            if (hardpoint.GetType() != typeof(WeaponHardpoint)) return;

            WeaponHardpoint weaponHardpoint = (WeaponHardpoint)hardpoint;

            weaponHardpoint.slot = slotCounter;

            slotCounter++;
        }
    }

    public void ToggleAfterburner(bool toggle)
    {
        if (toggle) afterburnerHardpoint.Activate();
        else afterburnerHardpoint.Disable();
    }

    private void Update()
    {
        RechargeEnergy();
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
            if (hardpoint.active) hardpoint.Fire();
    }

    public void FireHardpoint(int hardpointSlot)
    {
        if (weaponHardpoints.ContainsKey(hardpointSlot) && weaponHardpoints[hardpointSlot] != null)
            weaponHardpoints[hardpointSlot].Fire();
    }

    private void RechargeEnergy()
    {
        energy = Mathf.Clamp(energy, 0, energyCapacity);
        energy = Mathf.MoveTowards(energy, energyCapacity, chargeRate * Time.deltaTime);
    }

    public void MountLoadout(Loadout newLoadout)
    {
        DemountAll();

        foreach (Equipment equipment in newLoadout.equipment)
        {
            foreach(Hardpoint hardpoint in hardpoints)
            {
                if (hardpoint.IsMounted) continue;

                if (hardpoint.associatedEquipmentType == equipment.GetType())
                {
                    hardpoint.Mount(equipment);
                    break;
                }
            }
        }
    }
}
