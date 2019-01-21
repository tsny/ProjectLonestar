using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship/ShipPhysicsStats")]
public class ShipPhysicsStats : ScriptableObject
{
    public float turnSpeed = 1;

    public float maxNormalSpeed = 50;
    public float maxAfterburnSpeed = 100;
    public float maxDriftSpeed = 250;
    public float maxCruiseSpeed = 300;
    public float maxTotalSpeed = 1000;

    public float mass = 1;
    public float drag = 1;
    public float driftMass = .5f;
    public float driftDrag = .5f;

    public static void ClampShipVelocity(Rigidbody rb, ShipPhysicsStats stats, CruiseState state)
    {
        float currentMaxSpeed = 0;

        switch (state)
        {
            case CruiseState.Off:
                currentMaxSpeed = stats.maxAfterburnSpeed;
                break;

            case CruiseState.Charging:
                currentMaxSpeed = stats.maxNormalSpeed;
                break;

            case CruiseState.On:
                currentMaxSpeed = stats.maxCruiseSpeed;
                break;

            case CruiseState.Disrupted:
                currentMaxSpeed = stats.maxAfterburnSpeed;
                break;

            default:
                break;
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, currentMaxSpeed);
    }

    public static void HandleDrifting(Rigidbody rb, ShipPhysicsStats stats, bool isDrifting)
    {
        if (isDrifting)
        {
            rb.mass = stats.driftMass;
            rb.drag = stats.driftDrag;
        }

        else
        {
            rb.mass = stats.mass;
            rb.drag = stats.drag;
        }
    }
}
