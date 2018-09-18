using CommandTerminal;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : WorldObject
{
    [Header("Stats")]
    public PilotDetails pilotDetails;
    public EngineStats engineStats;
    public ShipDetails shipDetails;
    public ShipStats stats;

    [Header("Ship Components")]
    public HardpointSystem hardpointSystem;
    public Vector3 aimPosition;
    public Engine engine;
    public CruiseEngine cruiseEngine;
    public ShipCamera shipCam;
    public Rigidbody rb;
    public List<ShipComponent> components;

    public delegate void PossessionEventHandler(PlayerController pc, Ship sender, bool possessed);
    public event PossessionEventHandler Possession;

    protected override void Awake()
    {
        hardpointSystem = GetComponentInChildren<HardpointSystem>();
        cruiseEngine = GetComponentInChildren<CruiseEngine>();
        shipCam = GetComponentInChildren<ShipCamera>();
        engine = GetComponentInChildren<Engine>();
        rb = GetComponentInChildren<Rigidbody>();

        hullHealth = hullFullHealth;

        if (stats == null)
        {
            stats = ScriptableObject.CreateInstance<ShipStats>();
            print("No ship stats found, assigning default ship values...");
        }

        GetComponentsInChildren(true, components);

        foreach (var component in components)
        {
            component.InitShipComponent(this, stats);
        }

        base.Awake();
    }

    public void ChangePossession(PlayerController pc, bool possessed)
    {
        name = possessed ? "PLAYER SHIP" : NameGenerator.GenerateFullName();

        tag = possessed ? "Player" : "Untagged";

        foreach(Transform transform in transform)
        {
            if (possessed)
            {
                transform.gameObject.layer = LayerMask.NameToLayer("Player");
            }

            else
            {
                transform.gameObject.layer = 0;
            }
        }

        OnPossession(pc, possessed);
    }

    protected void OnPossession(PlayerController pc, bool possessed)
    {
        if (Possession != null) Possession(pc, this, possessed);
    }

    public override void TakeDamage(Weapon weapon)
    {
        if (invulnerable) return;

        if (hardpointSystem.shieldHardpoint.IsOnline)
        {
            hardpointSystem.shieldHardpoint.TakeDamage(weapon);
            OnTookDamage(true, weapon.shieldDamage);
            return;
        }

        hullHealth -= weapon.hullDamage;
        OnTookDamage(false, weapon.hullDamage);

        if (hullHealth <= 0) Die();
    }

    protected override void SetHierarchyName()
    {
    }

    private void FixedUpdate()
    {
        if (cruiseEngine == null) return;

        float currentMaxSpeed = 0;

        switch (cruiseEngine.State)
        {
            case CruiseEngine.CruiseState.Off:
                currentMaxSpeed = engineStats.maxAfterburnSpeed;
                break;

            case CruiseEngine.CruiseState.Charging:
                currentMaxSpeed = engineStats.maxNormalSpeed;
                break;

            case CruiseEngine.CruiseState.On:
                currentMaxSpeed = engineStats.maxCruiseSpeed;
                break;

            case CruiseEngine.CruiseState.Disrupted:
                currentMaxSpeed = engineStats.maxAfterburnSpeed;
                break;

            default:
                break;
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, currentMaxSpeed);
    }
}