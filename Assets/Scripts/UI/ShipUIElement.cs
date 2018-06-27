using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    public PlayerController playerController;
    public Ship ship;

    protected virtual void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.ShipPossessed += HandlePossessed;
        playerController.ShipUnpossessed += HandleUnpossessed;
    }

    protected virtual void HandleUnpossessed(PlayerController sender, Ship oldShip)
    {
        ship = null;
    }

    protected virtual void HandlePossessed(PlayerController sender, Ship newShip)
    {
        ship = newShip;
    }
}
