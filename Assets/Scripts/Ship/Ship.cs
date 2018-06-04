using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : WorldObject
{
    [Header("Details")]
    protected string pilotFirstName = "Pilot First Name";
    protected string pilotLastName = "Pilot Last Name";
    public string shipName = "TestShip";
    public ShipClass shipClass = ShipClass.Light;
    public Faction faction = Faction.LibertyNavy;

    [Header("States")]
    public int cargoHoldCapacity = 30;

    [Header("References")]
    public List<ShipComponent> shipComponents;
    public HardpointSystem hardpointSystem;
    public Vector3 AimPosition;
    public ShipStats stats;

    public ShipMovement shipMovement;
    public Inventory inventory;

    public ParticleSystem dustParticleSystem;

    public delegate void DamageTakenEventHandler(WorldObject sender);
    public event DamageTakenEventHandler TookDamage;

    public bool CanFire
    {
        get
        {
            return !(shipMovement.engineState == EngineState.Charging || shipMovement.engineState == EngineState.Cruise);
        }
    }

    private Ship()
    {
        worldObjectType = WorldObjectType.Ship;
    }

    public override void SetupTargetIndicator(TargetIndicator indicator)
    {
        indicator.header.text = pilotLastName + " " + faction;
        indicator.targetHealth = hullHealth;
        indicator.targetShield = hardpointSystem.shieldHardpoint.health;

        TookDamage += indicator.HandleTargetDamaged;
    }

    protected override void SetName()
    {
        pilotFirstName = NameGenerator.Generate(Gender.Male).First;
        pilotLastName = NameGenerator.Generate(Gender.Male).Last;

        name =  pilotLastName + " - " + shipName + " - " + faction; 
    }

    protected override void Awake()
    {
        base.Awake();

        shipComponents.AddRange(GetComponentsInChildren<ShipComponent>(true));
        hardpointSystem = GetComponentInChildren<HardpointSystem>();
        shipMovement = GetComponent<ShipMovement>();
        inventory = GetComponentInChildren<Inventory>();

        hullHealth = hullFullHealth;
    }

    public override void TakeDamage(Weapon weapon)
    {
        if (invulnerable) return;

        if (!hardpointSystem.shieldHardpoint.IsOnline)
        {
            hullHealth -= weapon.hullDamage;

            if (hullHealth <= 0)
            {
                Explode();
            } 
        }

        else hardpointSystem.shieldHardpoint.TakeDamage(weapon);

        if (TookDamage != null) TookDamage(this);
    }

    public void Explode()
    {
        // Play FX
        OnDestroyed();
        Destroy(gameObject);
    }
}




