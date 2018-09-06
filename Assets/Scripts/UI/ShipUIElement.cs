using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    protected PlayerController playerController;
    protected Ship ship;

    protected virtual void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.Possession += HandlePossession;
        enabled = false;

        if (playerController.controlledShip != null)
        {
            HandlePossession(new PossessionEventArgs(playerController.controlledShip, null, playerController));
        }
    }

    protected void OnDestroy()
    {
        playerController.Possession -= HandlePossession;
    }

    private void HandlePossession(PossessionEventArgs args)
    {
        if (args.PossessingNewShip)
        {
            HandlePossessed(args.playerController, args.newShip);
        }

        else
        {
            HandleUnpossessed(args.playerController, args.oldShip);
        }
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
