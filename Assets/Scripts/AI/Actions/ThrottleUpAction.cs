using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Actions/ThrottleUp")]
public class ThrottleUpAction : FLAction
{
    public override void Act(StateController controller)
    {
        if (controller.ship.shipMovement.throttle < 1)
        {
            ThrottleUp(controller);
        }
    }

    private void ThrottleUp(StateController controller)
    {
        controller.ship.shipMovement.ThrottleUp();
    }
}
