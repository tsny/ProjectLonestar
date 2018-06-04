using UnityEngine;
using System.Collections;

public class ShipUIElement : MonoBehaviour
{
    public PlayerController playerController;

    protected virtual void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.ShipPossessed += HandlePossession;
    }

    protected virtual void HandlePossession(PlayerController sender, PossessionEventArgs e)
    {

    }
}
