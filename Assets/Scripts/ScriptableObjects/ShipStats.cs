using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship/ShipStats")]
public class ShipStats : ScriptableObject
{
    public ShipClass shipClass = ShipClass.Light;
    public HullType hullType = HullType.Light;

    public float energyCapacity = 100;
    public float energyChargeRate = 3;
    public float maxHealth = 100;
    public float cargoCapacity = 50;

    private float _power = 100;
    public float Power
    {
        get { return _power; }
        set
        {
            value = Mathf.Clamp(value, 1, 1000);
            _power = value;
        }
    }
    public int level = 1;
}
