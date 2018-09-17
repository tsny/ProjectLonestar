using CommandTerminal;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : WorldObject
{
    [Header("Details")]
    protected string pilotFirstName = "First Name";
    protected string pilotLastName = "Last Name";

    [Header("References")]
    public HardpointSystem hardpointSystem;
    public Vector3 aimPosition;
    public ShipStats stats;
    public Engine engine;
    public CruiseEngine cruiseEngine;
    public ShipCamera shipCam;
    public Rigidbody rb;
    public List<ShipComponent> components;
    public Inventory inventory;

    public delegate void PossessionEventHandler(PlayerController pc, Ship sender, bool possessed);
    public event PossessionEventHandler Possession;

    protected override void Awake()
    {
        hardpointSystem = GetComponentInChildren<HardpointSystem>();
        cruiseEngine = GetComponentInChildren<CruiseEngine>();
        inventory = GetComponentInChildren<Inventory>();
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

    public override string ToStringForScannerEntry()
    {
        return pilotFirstName + " - " + stats.shipName;
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

        if (hullHealth <= 0) Die();

        OnTookDamage(false, weapon.hullDamage);
    }

    protected override void SetHierarchyName()
    {
        pilotFirstName = NameGenerator.Generate(Gender.Male).First;
        pilotLastName = NameGenerator.Generate(Gender.Male).Last;

        name =  pilotLastName + " - " + stats.shipName; 
    }

}




