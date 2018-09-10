using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEffectManager : ShipComponent
{
    public GameObject[] engineEffects;
    public GameObject chargeCruiseEffect;
    public GameObject fullCruiseEffect;
    public GameObject dustEffect;

    private ParticleSystem test;

    private Vector3 originalScale;

    protected override void Awake()
    {
        base.Awake();
        owningShip.GetComponent<ShipEngine>().CruiseChanged += HandleCruiseChanged;
    }

    private void HandleCruiseChanged(ShipEngine sender)
    {
        switch (sender.engineState)
        {
            case EngineState.Normal:
                chargeCruiseEffect.SetActive(false);
                fullCruiseEffect.SetActive(false);
                break;

            case EngineState.Charging:
                chargeCruiseEffect.SetActive(true);
                break;

            case EngineState.Cruise:
                chargeCruiseEffect.SetActive(false);
                fullCruiseEffect.SetActive(true);
                break;

            case EngineState.Drifting:
                chargeCruiseEffect.SetActive(false);
                fullCruiseEffect.SetActive(false);
                break;

            case EngineState.Reversing:
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

    protected override void HandlePossession(Ship sender, bool possessed)
    {
        base.HandlePossession(sender, possessed);

        dustEffect.SetActive(possessed);
    }

    private void Update()
    {
        //var speedFactor = shipPhysics.speed / shipPhysics.maxSpeed;
        foreach (var effect in engineEffects)
        {
            effect.transform.localScale = originalScale * owningShip.shipEngine.throttle;
        }
    }
}
