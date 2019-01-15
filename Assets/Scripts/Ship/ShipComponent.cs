using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class ShipComponent : MonoBehaviour
{
    protected Ship ship;

    public virtual void Initialize(Ship sender)
    {
        ship = sender;
    }

    public virtual void Deinitialize()
    {
        ship = null;
    }
}
