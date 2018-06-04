using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/InWeaponsRange")]
public class InWeaponsRangeDecision : FLDecision
{
    public override bool Decide(StateController controller)
    {
        return IsInWeaponsRange(controller);
    }

    private bool IsInWeaponsRange(StateController controller)
    {
        return Vector3.Distance(controller.targetShip.transform.position, controller.transform.position) < 5f;
    }
}
