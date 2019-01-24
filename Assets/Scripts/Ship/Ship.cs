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
    public List<Collider> colliders;

    private GameObject _shipBase;
    public GameObject ShipBase
    {
        get
        {
            return _shipBase;
        }
        set
        {
            // Maybe make this component based instead?
            if (_shipBase) DestroyImmediate(_shipBase.gameObject);
            _shipBase = Instantiate(value, transform);
            _shipBase.transform.localPosition = Vector3.zero;
            engine.ShipBaseTransform = _shipBase.transform;
            Init();
        }
    }

    public ParticleSystem deathFX;

    [Header("Other")]
    private bool _possessed;
    public bool Possessed { get { return _possessed; } }
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

    public void Init()
    {
        if (ShipBase == null)
        {
            ShipBase = GameSettings.Instance.defaultShipBase;
            return;
        }

        GetComponentsInChildren<Collider>(true, colliders);

        foreach (Collider coll in colliders)
        {
            var newLayer = _possessed ? LayerMask.NameToLayer("Player") : LayerMask.NameToLayer("Default");
            coll.gameObject.layer = newLayer;
            coll.tag = _possessed ? "Player" : "Untagged";
        }

        var components = GetComponentsInChildren<ShipComponent>();
        components.ToList().ForEach(x => x.Initialize(this));

        name = ShipBase.name;
    }

    private void Start()
    {
        engine.DriftingChange += HandleDriftingChange;
        engine.ThrottleChanged += HandleThrottleChange;
        cruiseEngine.CruiseStateChanged += HandleCruiseChange;
        health.HealthDepleted += HandleHealthDepleted;

        if (ShipBase == null)
            ShipBase = GameSettings.Instance.defaultShipBase;

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

        _possessed = possessed;
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
