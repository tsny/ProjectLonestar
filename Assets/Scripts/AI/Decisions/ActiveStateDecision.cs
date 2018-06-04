using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
public class ActiveStateDecision : FLDecision
{
    public override bool Decide(StateController controller)
    {
        return CheckActive(controller);
    }

    private bool CheckActive(StateController controller)
    {
        return controller.targetShip.enabled;
    }
}
