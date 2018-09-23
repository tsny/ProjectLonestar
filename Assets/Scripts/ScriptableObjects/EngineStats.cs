using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship/EngineStats")]
public class EngineStats : ScriptableObject
{
    public float turnSpeed = 1;
    public int enginePower = 1;
    public int strafePower = 1;
    public int reversePower = 1;
}
