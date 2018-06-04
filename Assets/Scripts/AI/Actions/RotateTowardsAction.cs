using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Actions/RotateTowards")]
public class RotateTowardsAction : FLAction
{
    public override void Act(StateController controller)
    {
        if (controller.HasTarget)
        {
            RotateTowards(controller);
        }
    }

    private void RotateTowards(StateController controller)
    {
        if (controller.targetShip != null)
        {
            controller.ship.shipMovement.RotateTowardsTarget(controller.targetShip.transform);
        }

        else if (controller.targetLoc != null)
        {
            controller.ship.shipMovement.RotateTowardsTarget(controller.targetLoc);
        }
    }
}
