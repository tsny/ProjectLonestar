using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    public Ship ship;

    public virtual void Init(Ship newShip)
    {
        if (ship != null)
            Clear();

        ship = newShip;
    }

    protected virtual void Clear()
    {
        ship = null;
    }
}
