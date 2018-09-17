using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Hardpoint : ShipComponent
{
    public delegate void MountedEventHandler(Hardpoint sender, Equipment equipment);
    public event MountedEventHandler Mounted;
    public event MountedEventHandler Demounted;

    public bool IsMounted
    {
        get
        {
            return CurrentEquipment != null;
        }
    }
    public bool OnCooldown
    {
        get
        {
            return cooldownCoroutine != null;
        }
    }

    public Equipment CurrentEquipment { get; private set; }
    private IEnumerator cooldownCoroutine;

    protected virtual void OnMounted(Equipment newEquipment)
    {
        if (Mounted != null) Mounted(this, newEquipment);
    }

    public virtual bool TryMount(Equipment newEquipment)
    {
        if (newEquipment == null || EquipmentMatchesHardpoint(newEquipment) == false)
        {
            return false;
        }

        if (IsMounted) Demount();

        CurrentEquipment = newEquipment;

        OnMounted(newEquipment);

        return true;
    }

    protected virtual bool EquipmentMatchesHardpoint(Equipment equipment)
    {
        return false;
    }

    public virtual void Demount()
    {
        var oldEquipment = CurrentEquipment;

        CurrentEquipment = null;

        if (Demounted != null) Demounted(this, oldEquipment);
    }

    protected virtual void StartCooldown()
    {
        cooldownCoroutine = Cooldown(CurrentEquipment.cooldownDuration);
        StartCoroutine(cooldownCoroutine);
    }

    protected virtual void EndCooldown()
    {
        StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = null;
    }

    protected IEnumerator Cooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cooldownCoroutine = null;
    }
}


