using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    public Ship ship;

    public virtual void SetShip(Ship newShip)
    {
        if (ship != null)
        {
            ClearShip();
        }

        ship = newShip;
    }

    protected virtual void ClearShip()
    {
        ship = null;
    }
}
