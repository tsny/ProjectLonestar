using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    public ShipController playerController;
    public Ship ship;

    protected virtual void Awake()
    {
        playerController = FindObjectOfType<ShipController>();
        playerController.ShipPossessed += HandlePossessed;
        playerController.ShipUnpossessed += HandleUnpossessed;
    }

    protected virtual void HandleUnpossessed(ShipController sender, Ship oldShip)
    {
        ship = null;
    }

    protected virtual void HandlePossessed(ShipController sender, Ship newShip)
    {
        ship = newShip;
    }
}
