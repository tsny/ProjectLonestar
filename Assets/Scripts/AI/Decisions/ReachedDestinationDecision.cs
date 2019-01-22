using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/ReachedDestination")]
public class ReachedDestinationDecision : FLDecision
{
    public override void Init()
    {

    }

    public override bool Decide(StateController controller)
    {
        if (!controller.HasTarget) return false;
        return (controller.DistanceToTarget < controller.gotoDistanceThreshold);
    }
}
