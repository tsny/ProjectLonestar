using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/TargetTooClose")]
public class TargetTooClose : FLDecision
{
    public override void Init()
    {

    }

    public override bool Decide(StateController controller)
    {
        if (controller.targetTrans == null) return false;

        return Vector3.Distance(controller.ship.transform.position, controller.targetTrans.position) <= controller.combatDistanceThreshold;
    }
}