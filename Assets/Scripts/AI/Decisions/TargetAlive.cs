using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/TargetAlive")]
public class TargetAlive : FLDecision
{
    public override void Init()
    {

    }

    public override bool Decide(StateController controller)
    {
        return controller.TargetShip != null;
    }
}