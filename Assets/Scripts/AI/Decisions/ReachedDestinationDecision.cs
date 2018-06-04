using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ReachedDestination")]
public class ReachedDestinationDecision : FLDecision
{
    public override bool Decide(StateController controller)
    {
        return ReachedDestination(controller);
    }

    private bool ReachedDestination(StateController controller)
    {
        float targetDistance = Vector3.Distance(controller.transform.position, controller.targetLoc.position);
        return (targetDistance < controller.acceptableDistance);
    }
}
