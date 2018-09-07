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
    public HardpointSystem hardpointSystem;
    public Vector3 AimPosition;
    public ShipStats stats;

    public ShipMovement shipMovement;
    public Inventory inventory;

    public ParticleSystem dustParticleSystem;

    public bool CanFire
    {
        get
        {
            return !(shipMovement.engineState == EngineState.Charging || shipMovement.engineState == EngineState.Cruise);
        }
    }

    public override void SetupTargetIndicator(TargetIndicator indicator)
    {
        indicator.header.text = pilotLastName + " " + faction;
        indicator.targetHealth = hullHealth;
        indicator.targetShield = hardpointSystem.shieldHardpoint.health;
    }

    protected override void GenerateName()
    {
        pilotFirstName = NameGenerator.Generate(Gender.Male).First;
        pilotLastName = NameGenerator.Generate(Gender.Male).Last;

        name =  pilotLastName + " - " + shipName + " - " + faction; 
    }

    protected override void Awake()
    {
        base.Awake();

        FindObjectOfType<PlayerController>().Possession += HandlePossession;

        hardpointSystem = GetComponentInChildren<HardpointSystem>();
        shipMovement = GetComponent<ShipMovement>();
        inventory = GetComponentInChildren<Inventory>();

        hullHealth = hullFullHealth;
    }

    private void HandlePossession(PossessionEventArgs args)
    {
        if (args.newShip == this)
        {
            name = "PLAYER SHIP - " + shipName;
            tag = "Player";

            dustParticleSystem.gameObject.SetActive(true);
            hardpointSystem.shieldHardpoint.gameObject.layer = LayerMask.NameToLayer("Player");
        }

        else if (args.oldShip == this)
        {
            GenerateName();
            tag = "Untagged";
            dustParticleSystem.gameObject.SetActive(false);
            hardpointSystem.shieldHardpoint.gameObject.layer = 0;
        }
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

        if (hullHealth <= 0)
        {
            FindObjectOfType<PlayerController>().Possession -= HandlePossession;
            Die();
        }

        OnTookDamage(false, weapon.hullDamage);
    }
}




