using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is just a test comment, messing with Git

[CreateAssetMenu]
public class Projectile : ScriptableObject
{
    public new string name;
    public float hullDamage;
    public float shieldDamage;
    public float thrust;
    public float range;
    public DamageType damageType;
}


