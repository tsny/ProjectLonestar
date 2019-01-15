using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/TargetAlive")]
public class TargetAlive : FLDecision
{
    public override bool Decide(StateController controller)
    {
        return controller.TargetShip != null;
    }
}