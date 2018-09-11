using System.Collections.Generic;
using UnityEngine;

public class TractorHardpoint : Hardpoint
{
    public TractorBeam tractor;

    public float range = 20;
    public float pullForce = 10;

    public Inventory inventory;

    public TractorHardpoint()
    {
        associatedEquipmentType = typeof(TractorBeam);
    }

    protected override void Awake()
    {
        base.Awake();
        hardpointSystem.tractorHardpoint = this;
    }

    public override void Mount(Equipment newEquipment)
    {
        base.Mount(newEquipment);

        tractor = newEquipment as TractorBeam;
        range = tractor.range;
    }

    public void TractorAllLoot()
    {
        if (!IsMounted) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliders)
        {
            Loot loot = collider.GetComponent<Loot>();

            if (loot == null) continue;

            loot.SetTarget(transform, pullForce);
        }
    }

}







