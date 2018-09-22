using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship/CruiseEngineStats")]
public class CruiseEngineStats : ScriptableObject
{
    public float thrust = 100;
    public float chargeDuration = 5;
    public int thrustMultiplier = 1;
}
