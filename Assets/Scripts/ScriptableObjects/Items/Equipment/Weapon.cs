using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/Weapon")]
public class Weapon : Equipment
{
    public string fullName;

    public float energyDraw;
    public float hullDamage;
    public float shieldDamage;
    public float thrust;
    public float range;

    public int rank;

    public DamageType damageType;

    public GameObject projectile;
    public AudioClip clip;
}