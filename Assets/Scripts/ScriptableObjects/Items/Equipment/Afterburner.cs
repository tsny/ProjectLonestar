using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/Afterburner")]
public class Afterburner : Equipment
{
    public float drainRate = 1;
    public float capacity = 100;
    public float thrust = 3;
    public float regenRate = 3;
}