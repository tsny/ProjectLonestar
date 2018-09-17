using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/Afterburner")]
public class Afterburner : Equipment
{
    public float drain;
    public float capacity;
    public float thrust;
    public float regenRate;
}