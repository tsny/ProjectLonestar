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

        ship.hardpointSystem.WeaponFired += HandleWeaponFired;
        //ship.hull.TookDamage += HandleTookDamage;
    }

    private void HandleTookDamage(MonoBehaviour sender, DamageEventArgs e)
    {
        SetFillAmounts();
    }

    private void HandleWeaponFired(WeaponHardpoint hardpointFired)
    {
        SetFillAmounts();
    }

    private void Update()
    {
        SetText();
    }

    private void SetFillAmounts()
    {
        //healthBarImage.fillAmount = ship.hull.health / ship.hull.maxHealth;
        //shieldBarImage.fillAmount = ship.hardpointSystem.shieldHardpoint.health / ship.hardpointSystem.shieldHardpoint.capacity;
        energyBarImage.fillAmount = ship.hardpointSystem.energy / ship.hardpointSystem.energyCapacity;
    }

    private void SetHealthFill()
    {

    }

    private void SetShieldFill()
    {

    }

    // TODO?
    private void SetAbilityFill()
    {

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
