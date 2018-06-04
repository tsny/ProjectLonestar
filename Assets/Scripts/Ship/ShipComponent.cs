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
    }
}
