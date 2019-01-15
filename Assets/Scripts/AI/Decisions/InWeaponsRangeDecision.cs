using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/InWeaponsRange")]
public class InWeaponsRangeDecision : FLDecision
{
    public override bool Decide(StateController controller)
    {
        return IsInWeaponsRange(controller);
    }

    private bool IsInWeaponsRange(StateController controller)
    {
        return Vector3.Distance(controller.TargetShip.transform.position, controller.transform.position) < 5f;
    }
}
