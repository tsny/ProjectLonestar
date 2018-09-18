using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class ShipComponent : MonoBehaviour
{
    protected Ship owningShip;

    public virtual void Setup(Ship sender)
    {
        owningShip = sender;
    }
}
