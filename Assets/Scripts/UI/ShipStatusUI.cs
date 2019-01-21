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

    public Afterburner afterburnerHardpoint;
    public Engine engine;

    public override void SetShip(Ship ship)
    {
        base.SetShip(ship);
        afterburnerHardpoint = ship.engine.aft;
        engine = ship.engine;
    }

    private void Update()
    {
        if (ship == null) return;
        SetFillAmounts();
        SetText();
    }

    private void SetFillAmounts()
    {
        healthBarImage.fillAmount = ship.health.health / ship.health.stats.maxHealth;
        shieldBarImage.fillAmount = ship.health.shield / ship.health.stats.maxShield;
        energyBarImage.fillAmount = ship.hpSys.energy / ship.hpSys.energyCapacity;
    }

    private void SetText()
    {
        speedText.text = "" + (int) engine.Speed + " kph";

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
