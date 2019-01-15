using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "Item/Weapon")]
public class WeaponStats : Equipment, ICloneable
{
    public float range = 200;
    public float energyDraw = 10;
    public float hullDamage = 10;
    public float shieldDamage = 10;
    public float thrust = 800;
    public DamageType damageType;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}