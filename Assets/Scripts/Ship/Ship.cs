using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, ITargetable, IDamageable 
{
    [Header("Stats")]
    public PilotDetails pilotDetails;
    public EngineStats engineStats;
    public ShipDetails shipDetails;
    public ShipPhysicsStats physicsStats;
    //public ShipStats stats;

    [Header("Ship Components")]
    public Health health;
    public HardpointSystem hardpointSystem;
    public Vector3 aimPosition;
    public Engine engine;
    public CruiseEngine cruiseEngine;
    public Rigidbody rb;
    public Collider[] colliders;

    public ParticleSystem deathFX;

    [Header("Other")]
    public Transform cameraPosition;
    public Transform firstPersonCameraPosition;

    public delegate void PossessionEventHandler(PlayerController pc, Ship sender, bool possessed);
    public delegate void EventHandler();

    public event PossessionEventHandler Possession;
    public event TargetEventHandler BecameTargetable;
    public event TargetEventHandler BecameUntargetable;
    public event EventHandler Died;

    private void OnBecameTargetable()
    {
        if (BecameTargetable != null) BecameTargetable(this);
    }

    protected void OnPossession(PlayerController pc, bool possessed)
    {
        if (Possession != null) Possession(pc, this, possessed);
    }

    private void Awake()
    {
        hardpointSystem = GetComponentInChildren<HardpointSystem>();
        cruiseEngine = GetComponentInChildren<CruiseEngine>();
        engine = GetComponentInChildren<Engine>();
        rb = GetComponentInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        health = Health.CreateInstance<Health>();
        health.Init();

        var components = GetComponentsInChildren<ShipComponent>();
        components.ToList().ForEach(x => x.Initialize(this));

        engine.DriftingChange += HandleDriftingChange;
        engine.ThrottleChanged += HandleThrottleChange;
        cruiseEngine.CruiseStateChanged += HandleCruiseChange;
        health.HealthDepleted += HandleHealthDepleted;
    }

    public void Init()
    {
        health = Health.CreateInstance<Health>();
        health.Init();
    }

    private void HandleCruiseChange(CruiseEngine sender, CruiseState newState)
    {
        if (newState == CruiseState.Charging) 
        {
            engine.Drifting = false;
        }
    }

    private void HandleThrottleChange(Engine sender, ThrottleChangeEventArgs e)
    {
        if (e.IsAccelerating == false)
        {
            cruiseEngine.StopAnyCruise();
        }
    }

    private void HandleDriftingChange(bool drifting)
    {
        if (cruiseEngine != null && drifting)
        {
            cruiseEngine.StopAnyCruise();
        }

        ShipPhysicsStats.HandleDrifting(rb, physicsStats, drifting);
    }

    private void HandleHealthDepleted()
    {
        if (BecameUntargetable != null) BecameUntargetable(this);
        Die();
    }

    public void SetPossessed(PlayerController pc, bool possessed)
    {
        name = possessed ? "PLAYER SHIP" : "NPC SHIP " + pilotDetails.firstName;
        tag = possessed ? "Player" : "Untagged";

        foreach(Transform transform in transform)
        {
            var newLayer = possessed ? LayerMask.NameToLayer("Player") : 0;
            transform.gameObject.layer = newLayer;
        }

        OnPossession(pc, possessed);
    }

    private void FixedUpdate()
    {
        ShipPhysicsStats.ClampShipVelocity(rb, physicsStats, cruiseEngine.State);
    }

    public void FireActiveWeapons(Vector3 target)
    {
        hardpointSystem.FireActiveWeapons(target);
    }

    public void FireActiveWeapons()
    {
        hardpointSystem.FireActiveWeapons(aimPosition);
    }

    public void SetupTargetIndicator(TargetIndicator indicator)
    {
        indicator.header.text = shipDetails.shipName;
		
		// Check if this ship has a shield and toggle shield bar based on that
        //hull.TookDamage += indicator.HandleTargetTookDamage;
    }

    public bool IsTargetable()
    {
		// Do some kind of check if ship has cloak
        // hasCloak ? false : true;

        return true;
    }

    public void Die()
    {
        // EVENT CALL HERE (ON DYING)

        if (deathFX != null)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Dying ship has no deathFX...");
        }

        if (Died != null) Died();
        Destroy(gameObject);
        // EVENT CALL ALSO? (ON DEATH)
    }

    public void TakeDamage(WeaponStats weapon)
    {
        health.TakeDamage(weapon);
    }
}
