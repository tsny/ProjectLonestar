using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/HealthTooLow")]
public class HealthTooLow : FLDecision
{
    public float healthThreshold = 20;
    public float chance = 1;

    public override void Init()
    {

    }

    public override bool Decide(StateController cont)
    {
        if (cont.ship.health.Amount < healthThreshold)
        {
            if (Utilities.Chance(chance))
            {
                return true;
            }            
        }

        return false;
    }
}