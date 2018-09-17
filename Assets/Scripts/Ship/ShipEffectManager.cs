using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEffectManager : ShipComponent
{
    public GameObject[] engineEffects;
    public GameObject chargeCruiseEffect;
    public GameObject fullCruiseEffect;
    public GameObject dustEffect;

    public float currentScale = 1;

    private Vector3 originalScale;

    private void Awake()
    {
        enabled = false;
    }

    public override void InitShipComponent(Ship sender, ShipStats stats)
    {
        base.InitShipComponent(sender, stats);
        sender.cruiseEngine.CruiseStateChanged += HandleCruiseChanged;
        sender.Possession += HandleOwnerPossession;

        chargeCruiseEffect.SetActive(false);
        fullCruiseEffect.SetActive(false);
    }

    private void HandleOwnerPossession(PlayerController pc, Ship sender, bool possessed)
    {
        dustEffect.SetActive(possessed);
    }

    private void HandleCruiseChanged(CruiseEngine sender)
    {
        switch (sender.State)
        {
            case CruiseEngine.CruiseState.Off:
                chargeCruiseEffect.SetActive(false);
                fullCruiseEffect.SetActive(false);
                break;

            case CruiseEngine.CruiseState.Charging:
                chargeCruiseEffect.SetActive(true);
                break;

            case CruiseEngine.CruiseState.On:
                chargeCruiseEffect.SetActive(false);
                fullCruiseEffect.SetActive(true);
                break;

            case CruiseEngine.CruiseState.Disrupted:
                chargeCruiseEffect.SetActive(false);
                fullCruiseEffect.SetActive(false);
                break;

            default:
                break;
        }
    }

    private void Start()
    {
        // TODO: make this keep track of all orignal scales for each engine effect
        originalScale = engineEffects[0].transform.localScale;
        chargeCruiseEffect.SetActive(false);
        fullCruiseEffect.SetActive(false);
    }

    private void Update()
    {
        foreach (var effect in engineEffects)
        {
            effect.transform.localScale = originalScale * owningShip.engine.Speed;
        }
    }
}
