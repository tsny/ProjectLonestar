using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipStatusUI : ShipUIElement
{
    public Image healthBarImage;
    public Image energyBarImage;
    public Image shieldBarImage;

    public Text shipSpeed;

    public ShipPhysics shipPhysics;

    private void Update()
    {
        if (ship == null) return;

        SetFillAmounts();

        shipSpeed.text = "" + (int) shipPhysics.speed;
    }

    protected override void HandlePossessed(ShipController sender, Ship newShip)
    {
        base.HandlePossessed(sender, newShip);

        shipPhysics = ship.GetComponent<ShipPhysics>();
    }

    protected override void HandleUnpossessed(ShipController sender, Ship oldShip)
    {
        base.HandleUnpossessed(sender, oldShip);

        shipPhysics = null;
    }

    private void SetFillAmounts()
    {
        healthBarImage.fillAmount = ship.hullHealth / ship.hullFullHealth;
        shieldBarImage.fillAmount = ship.hardpointSystem.shieldHardpoint.health / ship.hardpointSystem.shieldHardpoint.capacity;
        energyBarImage.fillAmount = ship.hardpointSystem.energy / ship.hardpointSystem.energyCapacity;
    }
}
