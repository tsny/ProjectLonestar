using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/Afterburner")]
public class AfterburnerStats : Equipment
{
    public float drainRate = 1;
    public float capacity = 100;
    public float thrust = 10000;
    public float regenRate = 3;
}