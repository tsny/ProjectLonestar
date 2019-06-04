using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShipStatusUI : ShipUIElement
{
    public Image healthBarImage;
    public Image energyBarImage;
    public Image shieldBarImage;

    public Text afterburnerText;
    public Text speedText;

    public Afterburner afterburnerHardpoint;
    public Engine engine;

    public override void Init(PlayerController pc)
    {
        base.Init(pc);
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
        healthBarImage.fillAmount = ship.health.Amount / ship.health.stats.maxHealth;
        shieldBarImage.fillAmount = ship.health.Shield / ship.health.stats.maxShield;
        energyBarImage.fillAmount = ship.energy / ship.energyCapacity;
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

    public override void OnPossessed(PlayerController pc, PossessionEventArgs e)
    {
        this.enabled = true;
    }

    public override void OnReleased(PlayerController pc, PossessionEventArgs e)
    {
        speedText.text = "";
        afterburnerText.text = "";
    }
}