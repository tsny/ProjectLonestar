using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/TargetTooClose")]
public class TargetTooClose : FLDecision
{
    public override void Init()
    {

    }

    public override bool Decide(StateController controller)
    {
        if (!controller.HasTarget) return false;

        return controller.DistanceToTarget <= controller.combatDistanceThreshold;
    }
}