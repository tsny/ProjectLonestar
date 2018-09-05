using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipStatusUI : ShipUIElement
{
    public Image healthBarImage;
    public Image energyBarImage;
    public Image shieldBarImage;

    public Text afterburnerText;
    public Text speedText;

    private ShipPhysics shipPhysics;

    private void Update()
    {
        SetFillAmounts();
        SetText();
    }

    protected override void HandlePossessed(PlayerController sender, Ship newShip)
    {
        base.HandlePossessed(sender, newShip);

        shipPhysics = ship.GetComponent<ShipPhysics>();
    }

    protected override void HandleUnpossessed(PlayerController sender, Ship oldShip)
    {
        base.HandleUnpossessed(sender, oldShip);

        shipPhysics = null;
        afterburnerText.text = "";
        speedText.text = "";
    }

    private void SetFillAmounts()
    {
        healthBarImage.fillAmount = ship.hullHealth / ship.hullFullHealth;
        shieldBarImage.fillAmount = ship.hardpointSystem.shieldHardpoint.health / ship.hardpointSystem.shieldHardpoint.capacity;
        energyBarImage.fillAmount = ship.hardpointSystem.energy / ship.hardpointSystem.energyCapacity;
    }

    private void SetText()
    {
        speedText.text = "" + (int) shipPhysics.speed + " kph";

        if (ship.hardpointSystem.afterburnerHardpoint != null && ship.hardpointSystem.afterburnerHardpoint.IsMounted)
        {
            afterburnerText.text = "Afterburner: " + (int) ship.hardpointSystem.afterburnerHardpoint.charge + "%";
        }

        else
        {
            afterburnerText.text = "Afterburner: OFFLINE";
        }
    }
}
