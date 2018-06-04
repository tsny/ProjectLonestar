using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(ShipMovement))]
public class ShipPhysics : ShipComponent
{
    [Header("--- Movement ---")]
    [Space(5)]

    public int enginePower = 100;
    public int strafePower = 100;
    public int reversePower = 25;
    public int cruisePower = 300;
    public int cruiseMultiplier = 1;
    public int afterburnerPower = 180;

    public float drag = 5;
    public float mass = 5;

    [Header("--- States ---")]
    [Space(5), ReadOnly]
    public float speed;

    private Rigidbody rb;
    private ShipMovement shipMovement;
    public float maxSpeed = 100f;

    public bool AtMaxSpeed
    {
        get
        {
            return (rb.velocity.magnitude > maxSpeed);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        shipMovement = owningShip.GetComponent<ShipMovement>();
    }

    private void UpdateRigidbody()
    {
        if (shipMovement.engineState == EngineState.Drifting)
        {
            rb.drag = 0.05f;
        }

        else
        {
            rb.drag = drag;
            rb.mass = mass;
        }
    }

    private void FixedUpdate()
    {
        UpdateForces();
    }

    private void UpdateForces()
    {
        UpdateRigidbody();

        ApplyStrafeForces();

        switch (shipMovement.engineState)
        {
            case EngineState.Normal:
            case EngineState.Charging:

                ApplyThrottleForces();
                break;

            case EngineState.Cruise:
                ApplyCruiseForces();
                break;

            case EngineState.Reversing:
                UpdateRigidbody();
                break;
        }

        ApplyAfterburnerForces();

        if (AtMaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        speed = Vector3.Dot(rb.velocity, transform.forward);
    }

    private void ApplyCruiseForces()
    {
        rb.AddForce(rb.transform.forward * cruisePower * cruiseMultiplier);
    }

    private void ApplyStrafeForces()
    {
        if (shipMovement.strafe != 0)
        {
            rb.AddForce(rb.transform.right * shipMovement.strafe * strafePower);
        }
    }

    private void ApplyThrottleForces()
    {
        if (shipMovement.throttle > 0)
        {
            rb.AddForce(rb.transform.forward * shipMovement.throttle * enginePower);
        }
    }

    private void ApplyAfterburnerForces()
    {
        if (owningShip.hardpointSystem.afterburnerHardpoint == null) return;

        if (owningShip.hardpointSystem.afterburnerHardpoint.engaged)
        {
            rb.AddForce(rb.transform.forward * afterburnerPower * owningShip.hardpointSystem.afterburnerHardpoint.thrust);
        }
    }
} 