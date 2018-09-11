using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class ShipComponent : MonoBehaviour
{
    protected Ship owningShip;

    protected virtual void Awake()
    {
        owningShip = GetComponentInParent<Ship>();

        if (owningShip == null)
        {
            print("Could not find owning ship, destroying self...");
            Destroy(gameObject);
        }

        owningShip.Possession += HandlePossession;
    }

    protected virtual void HandlePossession(Ship sender, bool possessed) { }
}
