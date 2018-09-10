using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    protected PlayerController playerController;
    protected Ship ship;

    protected virtual void Awake()
    {
        enabled = false;
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null) return;

        playerController.Possession += HandlePossession;

        // If this object is not created during a possession, run the handlePossession method for the existing ship
        if (playerController.controlledShip != null)
        {
            HandlePossession(new PossessionEventArgs(playerController.controlledShip, null, playerController));
        }
    }

    protected void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.Possession -= HandlePossession;
        }
    }

    private void HandlePossession(PossessionEventArgs args)
    {
        if (args.PossessingNewShip) HandlePossessed(args.playerController, args.newShip);

        else HandleUnpossessed(args.playerController, args.oldShip);
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
