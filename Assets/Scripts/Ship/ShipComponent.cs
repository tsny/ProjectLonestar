using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class ShipComponent : MonoBehaviour
{
    protected Ship ship;

    public virtual void Setup(Ship sender)
    {
        ship = sender;
    }
}
