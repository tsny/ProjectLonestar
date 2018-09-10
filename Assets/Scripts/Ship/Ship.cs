using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : WorldObject, IPossessable
{
    [Header("Details")]
    protected string pilotFirstName = "First Name";
    protected string pilotLastName = "Last Name";
    public string shipName = "TestShip";
    public ShipClass shipClass = ShipClass.Light;
    public Faction faction = Faction.LibertyNavy;

    [Header("States")]
    public int cargoHoldCapacity = 30;

    [Header("References")]
    public HardpointSystem hardpointSystem;
    public Vector3 AimPosition;
    public ShipStats stats;

    public ShipEngine shipEngine;
    public Inventory inventory;

    public ParticleSystem dustParticleSystem;

    public bool CanFire
    {
        get
        {
            return !(shipEngine.engineState == EngineState.Charging || shipEngine.engineState == EngineState.Cruise);
        }
    }

    public delegate void PossessionEventHandler(Ship sender, bool possessed);
    public event PossessionEventHandler Possession;

    protected override void Awake()
    {
        base.Awake();

        hardpointSystem = GetComponentInChildren<HardpointSystem>();
        shipEngine = GetComponent<ShipEngine>();
        inventory = GetComponentInChildren<Inventory>();

        hullHealth = hullFullHealth;
    }

    public void Possessed(PlayerController sender)
    {
        name = "PLAYER SHIP - " + shipName;
        tag = "Player";

        foreach(Transform transform in transform)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Player");
        }

        OnPossession(true);
    }

    public void UnPossessed(PlayerController sender)
    {
        GenerateName();
        tag = "Untagged";
        foreach(Transform transform in transform)
        {
            transform.gameObject.layer = 0;
        }

        OnPossession(false);
    }

    protected void OnPossession(bool possessed)
    {
        if (Possession != null) Possession(this, possessed);
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

        if (hullHealth <= 0) Die();

        OnTookDamage(false, weapon.hullDamage);
    }

    protected override void GenerateName()
    {
        pilotFirstName = NameGenerator.Generate(Gender.Male).First;
        pilotLastName = NameGenerator.Generate(Gender.Male).Last;

        name =  pilotLastName + " - " + shipName + " - " + faction; 
    }

}




