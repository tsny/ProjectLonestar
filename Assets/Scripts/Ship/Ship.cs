using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    [Header("Stats")]
    public PilotDetails pilotDetails;
    public ShipDetails shipDetails;

    // TODO: Make some of these into properties
    [Header("Ship Components")]
    public StateController ai;
    public Health health;
    public Vector3 aimPosition;
    public GameObject hardpointParent;
    public List<Gun> guns;
    public Engine engine;
    public CruiseEngine cruiseEngine;
    public Afterburner aft;
    public Rigidbody rb;
    public List<Collider> colliders;
    public TargetingInfo targetInfo;

    public bool CanFireWeapons
    {
        get
        {
            if (cruiseEngine == null) return true;

            switch (cruiseEngine.State)
            {
                case CruiseState.Off:
                case CruiseState.Disrupted:
                    return true;

                case CruiseState.Charging:
                case CruiseState.On:
                    return false;

                default:
                    return false;
            }
        }
    }

    private float avgWepRange;
    public float AverageWeaponRange
    {
        get
        {
            if (avgWepRange != 0)
            {
                float totalRange = 0;
                foreach (var gun in guns) totalRange += gun.stats.range;
                avgWepRange = totalRange / guns.Count;
            } 
            
            return avgWepRange;
        }
    }

    public bool CanAfterburn
    {
        get
        {
            var state = cruiseEngine.State;
            return (state == CruiseState.Off);
        }
    }

    [Header("Energy")]

    public Cooldown energyCooldown;
    public float energy = 100;
    public float energyCapacity = 100;
    public float chargeRate = .2f;

    public ParticleSystem deathFX;

    [Header("Other")]
    private bool _possessed;
    public bool Possessed { get { return _possessed; } }
    public Transform cameraPosition;
    public Transform firstPersonCameraPosition;
    public float shipCollisionForce = 10;
    public float collisionExplosionRadius = 10;
    private IEnumerator rotateCR;

    public delegate void PossessionEventHandler(PlayerController pc, Ship sender, bool possessed);
    public delegate void ShipEventHandler(Ship sender);
    public delegate void WeaponFiredEventHandler(Gun gunFired);

    public static event ShipEventHandler Spawned;
    public event WeaponFiredEventHandler WeaponFired;
    public event ShipEventHandler Died;
    public event PossessionEventHandler Possession;

    protected void OnPossession(PlayerController pc, bool possessed) { if (Possession != null) Possession(pc, this, possessed); }

    void Awake()
    {
        ai = Utilities.CheckComponent<StateController>(gameObject);

        targetInfo = Utilities.CheckComponent<TargetingInfo>(gameObject);
        var header = shipDetails.shipName + " - PILOTNAMEHERE";
        targetInfo.Init(header, health);

        hardpointParent.GetComponentsInChildren<Gun>(guns);
        GetComponentsInChildren<Collider>(colliders);
    }

    void Start()
    {
        engine.DriftingChange += HandleDriftingChange;
        engine.ThrottleChanged += HandleThrottleChange;
        cruiseEngine.CruiseStateChanged += HandleCruiseChange;
        health.HealthDepleted += HandleHealthDepleted;

        SetLayersAndTags(_possessed);

        if (Spawned != null) Spawned(this);
    }

    void Update()
    {
        if (!energyCooldown.IsDecrementing)
        {
            if (energy < energyCapacity)
            {
                energy += chargeRate;
                energy = Mathf.Clamp(energy, 0, energyCapacity);
            }
        }
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
            cruiseEngine.StopAnyCruise();
    }

    private void HandleDriftingChange(bool drifting)
    {
        if (cruiseEngine != null && drifting)
            cruiseEngine.StopAnyCruise();

        ShipPhysicsStats.HandleDrifting(rb, engine.physicsStats, drifting);
    }

    private void HandleHealthDepleted()
    {
        Die();
    }

    public void SetPossessed(PlayerController pc, bool possessed)
    {
        // Add the random pilot's name
        name = possessed ? "PLAYER SHIP" : "NPC SHIP"; 
        tag = possessed ? "Player" : "Ship";

        if (possessed) transform.SetSiblingIndex(0);

        SetLayersAndTags(possessed);

        targetInfo.targetable = !possessed;

        _possessed = possessed;
        OnPossession(pc, possessed);
    }

    private void SetLayersAndTags(bool possessed)
    {
        tag = possessed ? "Player" : "Ship";

        foreach (Collider coll in colliders)
        {
            var newLayer = possessed ? LayerMask.NameToLayer("Player") : LayerMask.NameToLayer("Default");
            coll.gameObject.layer = newLayer;
            coll.tag = possessed ? "Player" : "Ship";
        }
    }

    public void Die()
    {
        // EVENT CALL HERE (ON DYING)

        if (deathFX != null)
            Instantiate(deathFX, transform.position, Quaternion.identity);
        else
            Debug.LogWarning("Dying ship has no deathFX...");

        if (Died != null) Died(this);

        Destroy(gameObject);
        // EVENT CALL ALSO? (ON DEATH)
    }

    public void ToggleAfterburner(bool toggle)
    {
        if (toggle && CanAfterburn) aft.Activate();
        else aft.Deactivate();
    }

    public bool FireActiveWeapons(AimPosition aimPos)
    {
        var allFired = false;

        foreach (Gun gun in guns)
        {
            if (gun.IsActive) 
            {
                if (!FireWeaponHardpoint(gun, aimPos))
                    allFired = false;
            }
        }

        return allFired;
    }

    public bool FireWeaponHardpoint(int hardpointSlot, AimPosition aim)
    {
        hardpointSlot--;

        if (hardpointSlot < 0 || hardpointSlot > guns.Count) 
            return false;

        if (guns[hardpointSlot] != null)
        {
            return FireWeaponHardpoint(guns[hardpointSlot], aim);
        }

        return false;
    }

    public bool FireWeaponHardpoint(Gun gun, AimPosition aim)
    {
        if (CanFireWeapons == false || gun.stats == null) 
            return false;

        if (gun.stats.energyDraw < energy && gun.Fire(aim, colliders.ToArray()))
        {
            energy -= gun.stats.energyDraw;
            if (WeaponFired != null) WeaponFired(gun);
            energyCooldown.Begin(this);
            return true;
        }

        return false;
    }

    public void Rotate(char axis, float amount, float increment)
    {
        if (rotateCR != null) return;
        rotateCR = RotateRoutine(axis, amount, increment);
        StartCoroutine(rotateCR);
    }

    private IEnumerator RotateRoutine(char axis, float amount, float increment)
    {
        float rotated = 0;

        while (rotated < amount)
        {
            rotated += amount;
            switch (axis)
            {
                case 'R': engine.AddRoll(increment); break;
                case 'P': engine.AddPitch(increment); break;
                case 'Y': engine.AddYaw(increment); break;
            }
            yield return new WaitForFixedUpdate();
        }

        rotateCR = null;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            Vector3 cont = other.contacts[0].point;
            rb.AddExplosionForce(shipCollisionForce, cont, collisionExplosionRadius, 3, ForceMode.Impulse);
        }
    }
}
