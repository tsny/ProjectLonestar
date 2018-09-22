using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : Hardpoint
{ 
    public TractorBeamStats tractor;

    public void TractorAllLoot()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, tractor.range);

        foreach (Collider collider in colliders)
        {
            Loot loot = collider.GetComponent<Loot>();

            if (loot == null) continue;

            loot.SetTarget(transform, tractor.pullForce);
        }

        StartCooldown(tractor.cooldownDuration);
    }
}
