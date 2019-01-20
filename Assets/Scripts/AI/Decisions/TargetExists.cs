using UnityEngine;

[CreateAssetMenu(fileName = "TargetExists", menuName = "AI/Decisions/TargetExists")]
public class TargetExists : FLDecision
{
    public override bool Decide(StateController controller)
    {
        return controller.targetTrans != null;
    }

    public override void Init()
    {
        //throw new System.NotImplementedException();
    }
}