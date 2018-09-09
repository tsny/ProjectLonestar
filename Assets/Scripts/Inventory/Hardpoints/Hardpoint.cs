using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Hardpoint : ShipComponent
{
    public HardpointSystem hardpointSystem;

    public Type associatedEquipmentType = typeof(Equipment);

    public delegate void MountedEventHandler(Hardpoint sender);
    public event MountedEventHandler Mounted;
    public event MountedEventHandler Demounted;

    public bool IsMounted
    {
        get
        {
            return currentEquipment != null;
        }
    }
    public bool OnCooldown
    {
        get
        {
            return cooldownCoroutine != null;
        }
    }

    public Equipment CurrentEquipment
    {
        get
        {
            return currentEquipment;
        }

        set
        {
            Mount(value);
        }
    }

    private IEnumerator cooldownCoroutine;

    private Equipment currentEquipment;

    protected override void Awake()
    {
        base.Awake();
        hardpointSystem = GetComponentInParent<HardpointSystem>();
    }

    protected virtual void StartCooldown()
    {
        cooldownCoroutine = Cooldown(currentEquipment.cooldownDuration);
        StartCoroutine(cooldownCoroutine);
    }

    protected virtual void EndCooldown()
    {
        StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = null;
    }

    public virtual void Mount(Equipment newEquipment)
    {
        if (IsMounted) Demount();

        if (newEquipment.GetType() == associatedEquipmentType)
        {
            currentEquipment = newEquipment;
            if (Mounted != null) Mounted(this);
        }

        else
        {
            print("Tried to populate with type " + newEquipment.GetType() + " on a " + GetType() + " hardpoint");
        }
    }

    public virtual void Demount()
    {
        currentEquipment = null;
        if (Demounted != null) Demounted(this);
    }

    protected IEnumerator Cooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cooldownCoroutine = null;
    }
}


