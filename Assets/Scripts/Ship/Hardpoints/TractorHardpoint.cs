using System.Collections.Generic;
using UnityEngine;

public class TractorHardpoint : Hardpoint
{
    public TractorBeam Tractor
    {
        get
        {
            return CurrentEquipment as TractorBeam;
        }
    }

    public float range = 20;
    public float pullForce = 10;

    public Inventory inventory;

    protected override bool EquipmentMatchesHardpoint(Equipment equipment)
    {
        return equipment is TractorBeam;
    }

    protected override void OnMounted(Equipment newEquipment)
    {
        base.OnMounted(newEquipment);

        range = Tractor.range;
        pullForce = Tractor.pullForce;
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







