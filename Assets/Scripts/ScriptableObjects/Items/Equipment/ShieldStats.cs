using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/Shield")]
public class ShieldStats : Equipment
{
    public float regenRate = 5;
    public float capacity = 100;

    public ShieldType type;
    public AudioClip hitClip;
}
