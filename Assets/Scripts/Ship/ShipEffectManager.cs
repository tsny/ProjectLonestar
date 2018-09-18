using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEffectManager : ShipComponent
{
    public ParticleSystem[] engineEffects;
    public ParticleSystem chargeCruiseEffect;
    public ParticleSystem fullCruiseEffect;
    public ParticleSystem dustEffect;

    public float currentScale = 1;

    private Vector3 originalScale;

    public override void InitShipComponent(Ship sender, ShipStats stats)
    {
        base.InitShipComponent(sender, stats);
        sender.cruiseEngine.CruiseStateChanged += HandleCruiseChanged;
        sender.Possession += HandleOwnerPossession;
    }

    private void HandleOwnerPossession(PlayerController pc, Ship sender, bool possessed)
    {
        if (possessed)
        {
            dustEffect.Play();
        }

        else
        {
            dustEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void HandleCruiseChanged(CruiseEngine sender)
    {
        switch (sender.State)
        {
            case CruiseEngine.CruiseState.Off:
                chargeCruiseEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                fullCruiseEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                break;

            case CruiseEngine.CruiseState.Charging:
                chargeCruiseEffect.Play();
                break;

            case CruiseEngine.CruiseState.On:
                chargeCruiseEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                fullCruiseEffect.Play();
                break;

            case CruiseEngine.CruiseState.Disrupted:
                chargeCruiseEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                fullCruiseEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                break;

            default:
                break;
        }
    }
}
