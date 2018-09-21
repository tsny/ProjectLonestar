using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship/ShipPhysicsStats")]
public class ShipPhysicsStats : ScriptableObject
{
    public float turnSpeed = 1;

    public float maxNormalSpeed = 40;
    public float maxAfterburnSpeed = 200;
    public float maxDriftSpeed = 250;
    public float maxCruiseSpeed = 300;
    public float maxTotalSpeed = 1000;

    public float mass = 1;
    public float drag = 1;
    public float driftMass = .5f;
    public float driftDrag = .5f;
}
