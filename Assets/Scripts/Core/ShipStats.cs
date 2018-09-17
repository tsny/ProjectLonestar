using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship")]
public class ShipStats : ScriptableObject
{
    public string shipName = "Ship";
    public string desc = "Description";

    public ShipClass shipClass = ShipClass.Light;
    public HullType hullType = HullType.Light;

    public float energyCapacity = 100;
    public float energyChargeRate = 3;
    public float maxHealth = 100;
    public float cargoCapacity = 50;

    public float turnSpeed = 1;
    public float maxNormalSpeed = 40;
    public float maxAfterburnSpeed = 200;
    public float maxDriftSpeed = 250;
    public float maxCruiseSpeed = 300;
    public float maxTotalSpeed = 1000;
    public float mass = 1;
    public float drag = 1;

    public int enginePower = 1;
    public int strafePower = 1;
    public int reversePower = 1;
    public int cruisePower = 1;
    public int cruiseMultiplier = 1;
}
