using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, ITargetable
{
    [Header("Stats")]
    public PilotDetails pilotDetails;
    public EngineStats engineStats;
    public ShipDetails shipDetails;
    public ShipPhysicsStats physicsStats;

    // TODO: Make some of these into properties
    [Header("Ship Components")]
    public Health health;
    public HardpointSystem hpSys;
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
    public delegate void ShipEventHandler(Ship sender);

    public static event ShipEventHandler Spawned;

    public event PossessionEventHandler Possession;
    public event TargetEventHandler BecameTargetable;
    public event TargetEventHandler BecameUntargetable;
    public event ShipEventHandler Died;

    private void OnBecameTargetable() { }

    protected void OnPossession(PlayerController pc, bool possessed) { if (Possession != null) Possession(pc, this, possessed); }

    private void Awake()
    {
        hpSys = GetComponentInChildren<HardpointSystem>();
        cruiseEngine = GetComponentInChildren<CruiseEngine>();
        engine = GetComponentInChildren<Engine>();
        rb = GetComponentInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        // Maybe make this required? CheckComponent<Health>
        health = GetComponentInChildren<Health>();

        var components = GetComponentsInChildren<ShipComponent>();
        components.ToList().ForEach(x => x.Initialize(this));

        engine.DriftingChange += HandleDriftingChange;
        engine.ThrottleChanged += HandleThrottleChange;
        cruiseEngine.CruiseStateChanged += HandleCruiseChange;
        health.HealthDepleted += HandleHealthDepleted;

        foreach (Collider coll in colliders)
        {
            var newLayer = LayerMask.NameToLayer("Default");
            coll.gameObject.layer = newLayer;
        }
    }

    private void Start()
    {
        if (Spawned != null) Spawned(this);
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
        //if (BecameUntargetable != null) BecameUntargetable(this);
        Die();
    }

    public void SetPossessed(PlayerController pc, bool possessed)
    {
        // Add the random pilot's name
        name = possessed ? "PLAYER SHIP" : "NPC SHIP"; 
        tag = possessed ? "Player" : "Ship";

        if (possessed)
        {
            transform.SetSiblingIndex(0);
        }

        foreach (Collider coll in colliders)
        {
            var newLayer = possessed ? LayerMask.NameToLayer("Player") : LayerMask.NameToLayer("Default");
            coll.gameObject.layer = newLayer;
            coll.tag = possessed ? "Player" : "Untagged";
        }

        OnPossession(pc, possessed);
    }

    public void SetupTargetIndicator(TargetIndicator indicator)
    {
        indicator.header.text = shipDetails.shipName;
		
		// Check if this ship has a shield and toggle shield bar based on that
        //hull.TookDamage += indicator.HandleTargetTookDamage;
    }

    public bool IsTargetable()
    {
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

        if (Died != null) Died(this);

        Destroy(gameObject);
        // EVENT CALL ALSO? (ON DEATH)
    }
}
