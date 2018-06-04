using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship")]
public class ShipStats : ScriptableObject
{
    public new string name = "Ship";
    public string desc = "Description";

    public ShipClass shipClass = ShipClass.Light;
    public HullType hullType = HullType.Light;

    public float maxHealth = 100;
    public float cargoCapacity = 50;

    public float turnSpeed = 1;
    public float mass = 1;
    public float maxSpeed = 100;
    public float drag = 1;

    public int enginePower = 1;
    public int strafePower = 1;
    public int reversePower = 1;
    public int cruisePower = 1;
    public int cruiseMultiplier = 1;
}
