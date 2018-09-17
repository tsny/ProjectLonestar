using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    public Ship ship;

    public virtual void SetShip(Ship ship)
    {
        this.ship = ship;
    }
}
