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

    public AfterburnerHardpoint afterburnerHardpoint;

    public override void SetShip(Ship ship)
    {
        base.SetShip(ship);

        enabled = true;

        if (ship.hardpointSystem.afterburnerHardpoint.IsMounted)
        {
            afterburnerHardpoint = ship.hardpointSystem.afterburnerHardpoint;
        }

        ship.hardpointSystem.WeaponFired += HandleWeaponFired;
        ship.TookDamage += HandleTookDamage;
    }

    private void HandleTookDamage(WorldObject sender, DamageEventArgs e)
    {
        SetFillAmounts();
    }

    private void HandleWeaponFired(Weapon weapon)
    {
        SetFillAmounts();
    }

    private void Update()
    {
        SetText();
    }

    private void SetFillAmounts()
    {
        healthBarImage.fillAmount = ship.hullHealth / ship.hullFullHealth;
        shieldBarImage.fillAmount = ship.hardpointSystem.shieldHardpoint.health / ship.hardpointSystem.shieldHardpoint.capacity;
        energyBarImage.fillAmount = ship.hardpointSystem.energy / ship.hardpointSystem.energyCapacity;
    }

    private void SetText()
    {
        speedText.text = "" + (int) ship.engine.Speed + " kph";

        if (afterburnerHardpoint != null)
        {
            afterburnerText.text = "Afterburner: " + (int) afterburnerHardpoint.charge + "%";
        }

        else
        {
            afterburnerText.text = "Afterburner: OFFLINE";
        }
    }
}
