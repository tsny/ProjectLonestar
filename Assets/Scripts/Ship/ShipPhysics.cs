using UnityEngine;

public class ShipPhysics : ShipComponent
{
    [Header("Movement")]
    [Space(5)]

    public int enginePower = 100;
    public int strafePower = 100;
    public int reversePower = 25;
    public int cruisePower = 300;
    public int cruiseMultiplier = 1;
    public int afterburnerPower = 180;

    public float drag = 5;
    public float mass = 5;

    public float Speed
    {
        get
        {
            return Vector3.Dot(rb.velocity, transform.forward);
        }
    }

    [Header("Mins and Maxes")]
    [Space(5)]

    public float maxNormalSpeed = 20;
    public float maxAfterburnSpeed = 50;
    public float maxDriftingSpeed = 50;
    public float maxCruiseSpeed = 100;
    public float maxTotalSpeed = 300;

    [Header("References")]
    [Space(5)]

    public Rigidbody rb;
    public Engine engine;
    public CruiseEngine cruiseEngine;

    private void Awake()
    {
        enabled = false;
    }

    public override void InitShipComponent(Ship sender, ShipStats stats)
    {
        base.InitShipComponent(sender, stats);

        maxNormalSpeed = stats.maxNormalSpeed;
        maxAfterburnSpeed = stats.maxAfterburnSpeed;
        maxDriftingSpeed = stats.maxDriftSpeed;
        maxCruiseSpeed = stats.maxCruiseSpeed;
        maxTotalSpeed = stats.maxTotalSpeed;

        cruiseEngine = sender.cruiseEngine;
        engine = sender.engine;
        rb = sender.GetComponentInChildren<Rigidbody>();

        engine.DriftingChange += HandleDriftingChange;

        enabled = true;
    }

    private void HandleDriftingChange(bool drifting)
    {
        if (drifting)
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
        ApplyStrafeForces();

        switch (cruiseEngine.State)
        {
            case CruiseEngine.CruiseState.Off:
            case CruiseEngine.CruiseState.Charging:
                ApplyThrottleForces();
                break;

            case CruiseEngine.CruiseState.On:
                ApplyCruiseForces();
                break;

            case CruiseEngine.CruiseState.Disrupted:
                ApplyThrottleForces();
                break;

            default:
                break;
        }

        //ApplyAfterburnerForces();

        ClampSpeed();
    }

    private void ClampSpeed()
    {
        var currentMaxSpeed = maxTotalSpeed;

        switch (owningShip.cruiseEngine.State)
        {
            case CruiseEngine.CruiseState.Off:
                break;

            case CruiseEngine.CruiseState.Charging:
                break;

            case CruiseEngine.CruiseState.On:
                break;

            case CruiseEngine.CruiseState.Disrupted:
                break;

            default:
                break;
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, currentMaxSpeed);
    }

    private void ApplyCruiseForces()
    {
        rb.AddForce(rb.transform.forward * cruisePower * cruiseMultiplier);
    }

    private void ApplyStrafeForces()
    {
        if (engine.Strafe != 0)
        {
            rb.AddForce(rb.transform.right * engine.Strafe * strafePower);
        }
    }

    private void ApplyThrottleForces()
    {
        if (engine.Throttle > 0)
        {
            rb.AddForce(rb.transform.forward * engine.Throttle * enginePower);
        }
    }

    private void ApplyAfterburnerForces(float afterburnerThrust)
    {
        rb.AddForce(rb.transform.forward * afterburnerThrust);
    }
} 