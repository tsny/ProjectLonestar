using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/ReachedDestination")]
public class ReachedDestinationDecision : FLDecision
{
    public override bool Decide(StateController controller)
    {
        if (controller.targetTrans == null) return false;

        float targetDistance = Vector3.Distance(controller.transform.position, controller.targetTrans.position);
        return (targetDistance < controller.gotoDistanceThreshold);
    }
}
