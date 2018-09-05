using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    protected PlayerController playerController;
    protected Ship ship;

    protected virtual void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.ShipPossessed += HandlePossessed;
        playerController.ShipUnpossessed += HandleUnpossessed;
        enabled = false;
    }

    protected virtual void HandleUnpossessed(PlayerController sender, Ship oldShip)
    {
        ship = null;
        enabled = false;
    }

    protected virtual void HandlePossessed(PlayerController sender, Ship newShip)
    {
        ship = newShip;
        enabled = true;
    }
}
