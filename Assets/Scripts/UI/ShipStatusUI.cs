using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipStatusUI : ShipUIElement
{
    public Image healthBarImage;
    public Image energyBarImage;
    public Image shieldBarImage;

    public Text shipSpeed;

    public Ship ship;
    public ShipPhysics shipPhysics;

    private void Update()
    {
        if (playerController == null) return;

        if (playerController.controlledShip != ship)
        {
            ship = playerController.controlledShip;
            shipPhysics = ship.GetComponent<ShipPhysics>();
        }

        SetFillAmounts();

        shipSpeed.text = "" + (int) shipPhysics.speed;
    }

    private void SetFillAmounts()
    {
        healthBarImage.fillAmount = ship.hullHealth / ship.hullFullHealth;
        shieldBarImage.fillAmount = ship.hardpointSystem.shieldHardpoint.health / ship.hardpointSystem.shieldHardpoint.capacity;
        energyBarImage.fillAmount = ship.hardpointSystem.energy / ship.hardpointSystem.energyCapacity;
    }
}
