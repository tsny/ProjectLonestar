using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/TargetHealth")]
public class TargetHealth : FLDecision
{
    public float healthThreshold = 50;

    public override void Init()
    {

    }

    public override bool Decide(StateController controller)
    {
        return controller.TargetShip.health.Amount <= healthThreshold;
    }
}
