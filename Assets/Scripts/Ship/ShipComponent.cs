using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class ShipComponent : MonoBehaviour
{
    protected Ship owningShip;

    public virtual void Initialize(Ship sender)
    {
        owningShip = sender;
    }

    public virtual void Deinitialize()
    {
        owningShip = null;
    }
}
