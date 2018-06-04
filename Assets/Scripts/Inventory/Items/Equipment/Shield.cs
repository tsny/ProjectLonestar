using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/Shield")]
public class Shield : Equipment
{
    public float regenRate;
    public float capacity;
    public ShieldType type;
    public AudioClip hitClip;
}
