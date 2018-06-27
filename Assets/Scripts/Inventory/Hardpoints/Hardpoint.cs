using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Hardpoint : ShipComponent
{
    public HardpointSystem hardpointSystem;

    private Equipment currentEquipment;
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
            return onCooldown;
        }

        set
        {
            if (value)
            {
                StopAllCoroutines();
                StartCoroutine(CooldownCoroutine());
            }

            else
            {
                StopCoroutine(CooldownCoroutine());
                onCooldown = false;
            }
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

    protected bool onCooldown;

    protected override void Awake()
    {
        base.Awake();
        hardpointSystem = GetComponentInParent<HardpointSystem>();
    }

    public virtual void Mount(Equipment newEquipment)
    {
        if (IsMounted) Demount();

        if (newEquipment.GetType() == associatedEquipmentType)
        {
            currentEquipment = newEquipment;
            if (Mounted != null) Mounted(this);
            return;
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

    protected IEnumerator CooldownCoroutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(currentEquipment.cooldownDuration);
        onCooldown = false;
    }
}


